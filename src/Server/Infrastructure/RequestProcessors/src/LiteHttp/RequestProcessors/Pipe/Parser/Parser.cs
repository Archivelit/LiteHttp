using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace LiteHttp.RequestProcessors.PipeContext.Parser;

#nullable disable
internal sealed class Parser
{
    // Some pre-allocated errors to prevent extra allocations
    private static readonly Error RequestLineSyntaxError =
        new Error(ParserErrors.InvalidRequestSyntax, "Request line has wrong format");
    private static readonly Error InvalidHeaderValueTypeError =
        new Error(ParserErrors.InvalidHeaderValue, ExceptionStrings.InvalidHeaderValueType);
    
    private readonly HttpContextBuilder _httpContextBuilder = new();
    private readonly IHeaderParser _headerParser = new DefaultHeaderParser();

    private ParsingState _parsingState = ParsingState.HeadersParsing;
    private HeaderCollection _headerCollection = new();
    private long _contentLength = 0;
    
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="requestPipe">Pipe contains bytes of entire request.</param>
    /// <returns><see cref="Result{TResult}"/> wrappee with result or exception wrapped</returns>
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
            if (!TryParseChunk(ref chunkReader, out var examined, out var error))
            {
                Debug.Assert(chunkReader.Consumed >= examined, "Consumed bytes does not correspond to examined bytes");

                requestPipe.Reader.AdvanceTo(chunkReader.Position);
                await requestPipe.Reader.CompleteAsync();
                return error;
            }

            requestPipe.Reader.AdvanceTo(unprocessedBytes.GetPosition(examined));

            if (_parsingState == ParsingState.Finished)
                break;

            var readResult = await requestPipe.Reader.ReadAsync();
        
            if (readResult.IsCompleted && readResult.Buffer.IsEmpty)
                break;

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
        _headerCollection = new();
    }

    /// <summary>
    /// Parses a chunk of data from the provided <paramref name="chunkReader"/>.
    /// </summary>
    /// <param name="chunkReader">The <see cref="SequenceReader{T}"/> instance that encapsulates data for sequential reading.</param>
    /// <param name="examined">The number of item processed by this method.</param>
    /// <param name="error">An error produced by the method, or null if the operation completed successfully.</param>
    /// <returns>True if parsing succeeded; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryParseChunk(ref SequenceReader<byte> chunkReader, out long examined, out Error? error)
    {
        examined = 0;
        
        switch (_parsingState)
        {
            case ParsingState.HeadersParsing:
            {
                while (TryReadLine(ref chunkReader, out var line))
                {
                    var result = ParseHeader(line);

                    if (!result.Success)
                    {
                        if (result.Error.Value.ErrorCode != HeaderParsingErrors.StateUpdateRequested.ErrorCode)
                        {
                            error = result.Error;
                            return false;
                        }

                        _parsingState = ParsingState.BodyParsing;
                        _httpContextBuilder.WithHeaders(_headerCollection);

                        var contentLengthUpdateResult = UpdateContentLength();                        
                        if (!contentLengthUpdateResult.Success)
                        {
                            error = contentLengthUpdateResult.Error;
                            return false;
                        }
                        chunkReader.Advance(1); // Advance to skip LF ('\n')
                        examined += line.Length;
                        break;
                    }
                    examined += line.Length;   
                }

                if (_parsingState == ParsingState.BodyParsing)
                {
                    goto case ParsingState.BodyParsing;
                }
            }
                break;
            
            // TODO: Implement reading and merging chunks of data from body
            case ParsingState.BodyParsing:
            {
                SkipToBodyStart(ref chunkReader);

                var body = chunkReader.UnreadSequence.Slice(
                    chunkReader.UnreadSequence.Start,
                    _contentLength);
                
                _httpContextBuilder.WithBody(body);

                examined += body.Length;

                _parsingState = ParsingState.Finished;
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

        if (!TryReadLine(ref sequenceReader, out var requestLine))
            return RequestLineSyntaxError;
            
        var result = ParseRequestLine(requestLine);

        if (!result.Success)
            return result.Error;

        return readResult.Buffer.Slice(requestLine.Length + 1); // +1 to skip LF ('\n')
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
            _httpContextBuilder.WithMethod(methodSequence.GetReadOnlyMemoryFromSequence());
            _httpContextBuilder.WithRoute(routeSequence.GetReadOnlyMemoryFromSequence());
            _httpContextBuilder.WithProtocolVersion(protocolVersionSequence.GetReadOnlyMemoryFromSequence());

            return Result.Successful;
        }

        return RequestLineSyntaxError;
    }

    /// <summary>
    /// Parses an HTTP header line and updates the internal header collection and parsing state as needed.
    /// </summary>
    /// <remarks>If the header parsing operation requests a state update, the method transitions to body
    /// parsing and updates the HTTP context with the parsed headers. Any errors encountered during parsing or state
    /// update are returned in the Result.</remarks>
    /// <param name="line">The sequence of bytes representing a single HTTP header line to parse.</param>
    /// <returns>A Result indicating the outcome of the header parsing operation. Returns Result.Successful if the header is
    /// parsed successfully or if a state update is performed; otherwise, returns a Result containing error information.</returns>
    private Result ParseHeader(ReadOnlySequence<byte> line)
    {
        var result = _headerParser.ParseHeader(line, _headerCollection);

        return result.Success 
            ? Result.Successful 
            : result.Error;
    }

    /// <summary>
    /// Updates the content length based on the parsed headers.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result UpdateContentLength()
    {
        if (_headerCollection.Headers.TryGetValue(HeadersAsBytes.ContentLength, out var contentLengthValue))
        {
            if (!long.TryParse(contentLengthValue.Span, out var result))
            {
                return InvalidHeaderValueTypeError;
            }
            _contentLength = result;
        }
        return Result.Successful;
    }

    /// <summary>
    /// Skips all CR (\r) and LF (\n) bytes until the start of the body.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SkipToBodyStart(ref SequenceReader<byte> sequenceReader)
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
    private static bool TryReadLine(ref SequenceReader<byte> sequenceReader, out ReadOnlySequence<byte> line) =>
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