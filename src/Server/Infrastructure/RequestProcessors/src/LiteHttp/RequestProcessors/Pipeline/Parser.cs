namespace LiteHttp.RequestProcessors.Pipeline;

using LiteHttp.Models.PipeContextModels;

#nullable disable
internal sealed class Parser
{
    private static readonly Error SRequestLineSyntaxError =
        new Error(ParserErrors.InvalidRequestSyntax, "Request line has wrong format");
    private static readonly Error SHeaderSyntaxError = 
        new Error(ParserErrors.InvalidRequestSyntax, "Header has wrong format");
    private static readonly Error SInvalidHeaderValueTypeError = 
        new Error(ParserErrors.InvalidHeaderValue, ExceptionStrings.InvalidHeaderValueType)
    
    private readonly HttpContextBuilder _httpContextBuilder = new();
    
    private ParsingState _parsingState = ParsingState.RequestLineParsing;
    private long _contentLength = 0;
    
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="requestPipe">Pipe contains bytes of entire request.</param>
    /// <returns><see cref="Result{TResult}"/> wrappee with result or exception wrapped</returns>
    [SkipLocalsInit]
    public async ValueTask<Result<HttpContext>> Parse(Pipe requestPipe)
    {
        _parsingState = ParsingState.RequestLineParsing;
        _contentLength = 0;
        _httpContextBuilder.Reset();
        
        while (true)
        {
            var result = await requestPipe.Reader.ReadAsync();
            
            var buffer = result.Buffer;
            
            var sequenceReader = new SequenceReader<byte>(buffer);
            
            while (sequenceReader.TryReadTo(out ReadOnlySequence<byte> line, RequestSymbolsAsBytes.NewLine, false))
            {
                if (_parsingState == ParsingState.BodyParsing)
                {
                    SkipToBodyStart(sequenceReader);
                    
                    if (_contentLength == 0)
                        _httpContextBuilder.WithBody(null);
                    else
                    {
                        _httpContextBuilder.WithBody(sequenceReader.UnreadSequence.Slice(
                            sequenceReader.UnreadSequence.Start,
                            _contentLength)
                        );
                    }
                }

                if (!TryParseLine(line, out var error))
                {
                    await requestPipe.Reader.CompleteAsync();

                    return new(error);
                }
                sequenceReader.Advance(1);
                requestPipe.Reader.AdvanceTo(line.Start);
            }
            
            if (result.IsCompleted)
                break;
        }

        await requestPipe.Reader.CompleteAsync();

        return new(_httpContextBuilder.Build());
    }

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
    private bool TryParseLine(ReadOnlySequence<byte> line, out Error? error)
    {
        switch (_parsingState)
        {
            case ParsingState.RequestLineParsing:
            {
                var result = ParseRequestLine(line);

                if (!result.Success)
                {
                    error = result.Error;
                    return false;
                }

                _parsingState = ParsingState.HeadersParsing;
            }
                break;

            case ParsingState.HeadersParsing:
            {
                var result = ParseHeader(line);

                if (!result.Success)
                {
                    error = result.Error;
                    return false;
                }
            }
                break;
        }

        error = null;
        return true;
    }

    [SkipLocalsInit]
    private Result ParseHeader(ReadOnlySequence<byte> line)
    {
        if (line.Length <= 2)
        {
            _parsingState = ParsingState.BodyParsing;
            return new();
        }

        var reader = new SequenceReader<byte>(line);
        
        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return new Result(SHeaderSyntaxError);

        reader.Advance(1); // Need to skip colon and space

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerValueSequence, RequestSymbolsAsBytes.CarriageReturnSymbol, true))
            return new Result(SHeaderSyntaxError);

        var headerTitleMemory = GetReadOnlyMemoryFromSequence(headerTitleSequence);
        var headerValueMemory = GetReadOnlyMemoryFromSequence(headerValueSequence);
        _httpContextBuilder.AddHeader(headerTitleMemory, headerValueMemory);

        if (IsContentLength(headerTitleMemory.Span))
        {
            if (!long.TryParse(headerValueMemory.Span, out var result))
                return new Result(SInvalidHeaderValueTypeError);
            
            _contentLength = result;
        }
        return new();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsContentLength(ReadOnlySpan<byte> headerTitleSpan)
    {
        const byte cByte = (byte)'c';
        const byte lByte = (byte)'l';
        const byte nByte = (byte)'h';
        
        return (headerTitleSpan[0] | 0x20) == cByte
               && (headerTitleSpan[8] | 0x20) == lByte
               && (headerTitleSpan[13] | 0x20) == nByte
               && ByteSpanComparerIgnoreCase.Equals(HeadersAsBytes.ContentLength, headerTitleSpan);
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

        return new(SRequestLineSyntaxError);
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

    private enum ParsingState
    {
        RequestLineParsing,
        HeadersParsing,
        BodyParsing
    }
}