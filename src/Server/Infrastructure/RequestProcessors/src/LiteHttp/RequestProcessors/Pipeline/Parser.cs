using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace LiteHttp.RequestProcessors.Pipeline;

#nullable disable
internal sealed class Parser
{
    private static readonly Error SRequestLineSyntaxError =
        new Error(ParserErrors.InvalidRequestSyntax, "Request line has wrong format");
    private static readonly Error SHeaderSyntaxError = 
        new Error(ParserErrors.InvalidRequestSyntax, "Header has wrong format");
    private static readonly Error SInvalidHeaderValueTypeError =
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
        // Reset parser state
        _parsingState = ParsingState.HeadersParsing;
        _contentLength = 0;
        _httpContextBuilder.Reset();
        
        var requestLineParsingResult = await ParseRequestLine(requestPipe.Reader);

        if (!requestLineParsingResult.Success)
            return requestLineParsingResult.Error;

        // ParseRequestLine returns unread sequence
        ReadOnlySequence<byte> unprocessedBytes = requestLineParsingResult.Value;
        
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

    private static bool TryReadLine(SequenceReader<byte> sequenceReader, out ReadOnlySequence<byte> line) => 
        sequenceReader.TryReadTo(out line, RequestSymbolsAsBytes.NewLine, false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SkipToBodyStart(SequenceReader<byte> sequenceReader)
    {
        while (sequenceReader.TryPeek(out var @byte))
        {
            if (@byte != '\r' && @byte != '\n')
            {
                sequenceReader.Rewind(1);
                break;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryParseChunk(SequenceReader<byte> chunkReader, out long examined, out Error? error)
    {
        examined = 0;
        
        switch (_parsingState)
        {
            case ParsingState.HeadersParsing:
            {
                while (TryReadLine(chunkReader, out var line))
                {
                    var result = ParseHeader(line);

                    if (!result.Success)
                    {
                        error = result.Error;
                        return false;
                    }
                    
                    chunkReader.Advance(1);
                    examined += line.Length;
                }

                goto case ParsingState.BodyParsing;
            }
            
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask<Result<ReadOnlySequence<byte>>> ParseRequestLine(PipeReader pipeReader)
    {
        var readResult = await pipeReader.ReadAsync();
            
        var sequenceReader = new SequenceReader<byte>(readResult.Buffer);

        if (!TryReadLine(sequenceReader, out var requestLine))
            return SRequestLineSyntaxError;
            
        var result = ParseRequestLine(requestLine);
        if (!result.Success)
            return result.Error;

        return readResult.Buffer.Slice(requestLine.Length);
    }
    
    [SkipLocalsInit]
    private Result ParseHeader(ReadOnlySequence<byte> line)
    {
        // Line integrity is already validated during input reading
        
        // Request separator line ("\r\n") encountered
        if (line.Length < 3)
        {
            _parsingState = ParsingState.BodyParsing;
            return new();
        }

        var reader = new SequenceReader<byte>(line);
        
        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return SHeaderSyntaxError;

        reader.Advance(1); // Skip space

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerValueSequence, RequestSymbolsAsBytes.CarriageReturnSymbol, true))
            return SHeaderSyntaxError;

        var headerTitleMemory = GetReadOnlyMemoryFromSequence(headerTitleSequence);
        var headerValueMemory = GetReadOnlyMemoryFromSequence(headerValueSequence);
        _httpContextBuilder.AddHeader(headerTitleMemory, headerValueMemory);

        if (!IsContentLength(headerTitleMemory.Span)) 
            return new();
        if (!long.TryParse(headerValueMemory.Span, out var result))
            return SInvalidHeaderValueTypeError;
            
        _contentLength = result;
        return new();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsContentLength(ReadOnlySpan<byte> headerTitleSpan)
    {
        const byte cByte = (byte)'c';
        const byte lByte = (byte)'l';
        const byte nByte = (byte)'h';
        
        return (headerTitleSpan.Length == 14
                && (headerTitleSpan[0] | 0x20) == cByte // Bitwise OR 0x20 converts uppercase letter to lowercase 
                && (headerTitleSpan[8] | 0x20) == lByte
                && (headerTitleSpan[13] | 0x20) == nByte
                && ByteSpanComparerIgnoreCase.Equals(HeadersAsBytes.ContentLength, headerTitleSpan));
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

            return new();
        }

        return SRequestLineSyntaxError;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlyMemory<byte> GetReadOnlyMemoryFromSequence(ReadOnlySequence<byte> methodSequence)
    {
        if (!SequenceMarshal.TryGetReadOnlyMemory(methodSequence, out var memory))
        {
            using var methodMemoryOwner = MemoryPool<byte>.Shared.Rent((int)methodSequence.Length);
            methodSequence.CopyTo(methodMemoryOwner.Memory.Span);

            return methodMemoryOwner.Memory;
        }

        return memory;
    }
    
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