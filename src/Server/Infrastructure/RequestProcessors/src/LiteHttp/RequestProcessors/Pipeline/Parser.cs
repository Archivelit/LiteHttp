using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace LiteHttp.RequestProcessors.Pipeline;

#nullable disable
internal sealed class Parser
{
    // Some pre-allocated errors to prevent extra allocations
    private static readonly Error RequestLineSyntaxError =
        new Error(ParserErrors.InvalidRequestSyntax, "Request line has wrong format");
    private static readonly Error HeaderSyntaxError = 
        new Error(ParserErrors.InvalidRequestSyntax, "Header has wrong format");
    private static readonly Error InvalidHeaderValueTypeError =
        new Error(ParserErrors.InvalidHeaderValue, ExceptionStrings.InvalidHeaderValueType);
    
    private readonly HttpContextBuilder _httpContextBuilder = new();
    
    private ParsingState _parsingState = ParsingState.HeadersParsing;
    private long _contentLength = 0;
    
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="requestPipe">Pipe contains bytes of entire request.</param>
    /// <returns><see cref="Result{TResult}"/> wrappee with result or exception wrapped</returns>
    [SkipLocalsInit]
    public async ValueTask<Result<HttpContext>> Parse(Pipe requestPipe)
    {
        ResetState();

        var requestLineParsingResult = await ParseRequestLine(requestPipe.Reader);

        if (!requestLineParsingResult.Success)
            return requestLineParsingResult.Error;

        // ParseRequestLine returns unread sequence
        var unprocessedBytes = requestLineParsingResult.Value;

        while (true)
        {
            var chunkReader = new SequenceReader<byte>(unprocessedBytes);
            if (!TryParseChunk(chunkReader, out var examined, out var error))
            {
                requestPipe.Reader.AdvanceTo(unprocessedBytes.Start, chunkReader.Position);
                await requestPipe.Reader.CompleteAsync();
                return error;
            }

            requestPipe.Reader.AdvanceTo(unprocessedBytes.Start, unprocessedBytes.GetPosition(examined));

            if (_parsingState == ParsingState.Finished)
                break;

            var readResult = await requestPipe.Reader.ReadAsync();
            unprocessedBytes = readResult.Buffer;
        }

        await requestPipe.Reader.CompleteAsync();

        return _httpContextBuilder.Build();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetState()
    {
        _parsingState = ParsingState.HeadersParsing;
        _contentLength = 0;
        _httpContextBuilder.Reset();
    }


    /// <summary>
    /// Parses a chunk of data from the provided <paramref name="chunkReader"/>.
    /// </summary>
    /// <param name="chunkReader">The <see cref="SequenceReader{T}"/> instance that encapsulates data for sequential reading.</param>
    /// <param name="examined">The number of item processed by this method.</param>
    /// <param name="error">An error produced by the method, or null if the operation completed successfully.</param>
    /// <returns>True if parsing succeeded; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryParseChunk(SequenceReader<byte> chunkReader, out long examined, out Error? error)
    {
        examined = 0;
        
        switch (_parsingState)
        {
            case ParsingState.HeadersParsing:
            {
                // TODO: can't parse all headers if reading stopped in middle of header instead LF
                while (TryReadLine(chunkReader, out var line))
                {
                    var result = ParseHeader(line);

                    if (!result.Success)
                    {
                        error = result.Error;
                        return false;
                    }
                    
                    // For skip LF symbol due to AdvancePastDelimiter: true in TryReadLine method
                    chunkReader.Advance(1);
                    examined += line.Length;
                }

                _parsingState = ParsingState.BodyParsing;
            }
                break;
            
            // TODO: Implement reading and merging chunks of data from body
            case ParsingState.BodyParsing:
            {
                SkipToBodyStart(chunkReader);

                var body = chunkReader.UnreadSequence.Slice(
                    chunkReader.UnreadSequence.Start,
                    _contentLength);
                
                _httpContextBuilder.WithBody(body);

                // Value can be assigned because we process body 1 time only
                examined = body.Length;
            }
                break;
        }

        error = null;
        return true;
    }

    /// <summary>
    /// Parses the HTTP request line from the specified pipe reader asynchronously.
    /// </summary>
    /// <param name="pipeReader">The <see cref="PipeReader"/> instance from which to read the HTTP request line. The reader must be positioned at
    /// the start of the request line.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous parse operation. The result contains a <see
    /// cref="ReadOnlySequence{Byte}"/> representing the buffer after the request line if parsing succeeds; otherwise,
    /// contains an error result indicating the reason for failure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask<Result<ReadOnlySequence<byte>>> ParseRequestLine(PipeReader pipeReader)
    {
        var readResult = await pipeReader.ReadAsync();
            
        var sequenceReader = new SequenceReader<byte>(readResult.Buffer);

        if (!TryReadLine(sequenceReader, out var requestLine))
            return RequestLineSyntaxError;
            
        var result = ParseRequestLine(requestLine);

        if (!result.Success)
            return result.Error;

        return readResult.Buffer.Slice(requestLine.Length);
    }

    /// <summary>
    /// Extracts request method, route and protocol version from request line
    /// </summary>
    /// <param name="line">Request line of the entire request</param>
    /// <returns>Error if operation was not success, otherwise null</returns>
    private Result ParseRequestLine(ReadOnlySequence<byte> line)
    {
        var reader = new SequenceReader<byte>(line);

        if (reader.TryReadTo(out ReadOnlySequence<byte> methodSequence, RequestSymbolsAsBytes.Space, true)
            && reader.TryReadTo(out ReadOnlySequence<byte> routeSequence, RequestSymbolsAsBytes.Space, true)
            && reader.TryReadTo(out ReadOnlySequence<byte> protocolVersionSequence, RequestSymbolsAsBytes.CarriageReturnSymbol,
                true))
        {
            _httpContextBuilder.WithMethod(GetReadOnlyMemoryFromSequence(methodSequence));
            _httpContextBuilder.WithRoute(GetReadOnlyMemoryFromSequence(routeSequence));
            _httpContextBuilder.WithProtocolVersion(GetReadOnlyMemoryFromSequence(protocolVersionSequence));

            return Result.Successful;
        }

        return RequestLineSyntaxError;
    }

    /// <summary>
    /// Parses a single HTTP header line from the provided byte sequence and updates the current parsing state and
    /// context accordingly.
    /// </summary>
    /// <remarks>If the provided line is a separator (i.e., an empty line or only contains a line break), the
    /// method transitions the parser to body parsing state. For valid headers, the method adds the header to the HTTP
    /// context. Also, current parser version <strong>does not</strong> support the following header syntax: Title:Value
    /// If the header is Content-Length, the value is parsed and stored; an error result is returned if the
    /// value is not a valid integer.</remarks>
    /// <param name="line">The sequence of bytes representing a single header line to parse. Must not be empty and should be properly
    /// formatted according to HTTP header syntax.</param>
    /// <returns>A result indicating the outcome of the header parsing operation. Returns a syntax error result if the header is
    /// malformed, or an invalid value type error if the Content-Length header value is not a valid number.</returns>
    private Result ParseHeader(ReadOnlySequence<byte> line)
    {
        // Line integrity is already validated during input reading

        // Request separator line ("\r\n") encountered
        if (line.Length < 3)
        {
            _parsingState = ParsingState.BodyParsing;
            return Result.Successful;
        }

        var reader = new SequenceReader<byte>(line);

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return HeaderSyntaxError;

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerValueSequence, RequestSymbolsAsBytes.CarriageReturnSymbol, true))
            return HeaderSyntaxError;

        var headerTitleMemory = GetReadOnlyMemoryFromSequence(headerTitleSequence);
        var headerValueMemory = GetReadOnlyMemoryFromSequence(headerValueSequence);
        _httpContextBuilder.AddHeader(headerTitleMemory, TrimStart(headerValueMemory));

        if (!IsContentLength(headerTitleMemory.Span))
            return Result.Successful;
        if (!long.TryParse(headerValueMemory.Span, out var result))
            return InvalidHeaderValueTypeError;

        _contentLength = result;
        return Result.Successful;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlyMemory<byte> GetReadOnlyMemoryFromSequence(ReadOnlySequence<byte> methodSequence)
    {
        if (SequenceMarshal.TryGetReadOnlyMemory(methodSequence, out var memory))
        {
            return memory;
        }

        using var methodMemoryOwner = MemoryPool<byte>.Shared.Rent((int)methodSequence.Length);
        methodSequence.CopyTo(methodMemoryOwner.Memory.Span);

        return methodMemoryOwner.Memory;
    }

    /// <summary>
    /// Determines whether the specified header title span represents the HTTP 'Content-Length' header, using a
    /// case-insensitive comparison.
    /// </summary>
    /// <remarks>This method performs a case-insensitive comparison and expects the header name to be exactly
    /// 14 bytes long, corresponding to the ASCII encoding of 'Content-Length'.</remarks>
    /// <param name="headerTitleSpan">A read-only span of bytes containing the header name to compare. The span must represent an ASCII-encoded header
    /// name.</param>
    /// <returns>true if the span matches 'Content-Length' (case-insensitive); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsContentLength(ReadOnlySpan<byte> headerTitleSpan)
    {
        const byte cByte = (byte)'c';
        const byte lByte = (byte)'l';
        const byte nByte = (byte)'h';

        return headerTitleSpan.Length == 14
                && (headerTitleSpan[0] | 0x20) == cByte // Bitwise OR 0x20 converts uppercase letter to lowercase 
                && (headerTitleSpan[8] | 0x20) == lByte
                && (headerTitleSpan[13] | 0x20) == nByte
                && ByteSpanComparerIgnoreCase.Equals(HeadersAsBytes.ContentLength, headerTitleSpan);
    }

    /// <summary>
    /// Skips all CR (\r) and LF (\n) bytes until the start of the body.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SkipToBodyStart(SequenceReader<byte> sequenceReader)
    {
        for (var bytesToSkip = 0; sequenceReader.TryPeek(out var @byte); bytesToSkip++)
        {
            if (@byte != '\r' && @byte != '\n')
            {
                sequenceReader.Advance(bytesToSkip);
                break;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlyMemory<byte> TrimStart(ReadOnlyMemory<byte> memory)
    {
        int bytesToSkip = 0, current = 0;
        while (current < memory.Length && memory.Span[current] == ' ')
        {
            bytesToSkip += 1;
            current += 1;
        }

        return memory[bytesToSkip..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryReadLine(SequenceReader<byte> sequenceReader, out ReadOnlySequence<byte> line) =>
        sequenceReader.TryReadTo(out line, RequestSymbolsAsBytes.NewLine, false);

    /// <summary>
    /// Represents parsing states of parser. Here is no request line parsing state because it's only 1 time operation
    /// </summary>
    private enum ParsingState
    {
        HeadersParsing,
        BodyParsing,
        Finished
    }
}