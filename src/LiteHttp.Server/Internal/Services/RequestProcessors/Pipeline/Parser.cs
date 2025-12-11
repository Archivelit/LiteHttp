namespace LiteHttp.RequestProcessors.Pipeline;

using LiteHttp.Models.PipeContextModels;

#nullable disable
// Issue codes and their meaning:
// 2 - Request line has wrong format
// 3 - Header has wrong format
internal sealed class Parser
{
    private readonly HttpContextBuilder _httpContextBuilder = new();
    
    private ParsingState _parsingState = ParsingState.RequestLineParsing;
    
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="requestPipe">Pipe contains bytes of entire request.</param>
    /// <returns><see cref="Result{TResult}"/> wrappee with result or exception wrapped</returns>
    [SkipLocalsInit]
    public async Task<Result<HttpContext>> Parse(Pipe requestPipe)
    {
        _parsingState = ParsingState.RequestLineParsing;
        _httpContextBuilder.Reset();
        
        while (true)
        {
            var result = await requestPipe.Reader.ReadAsync();
            
            var buffer = result.Buffer;
            
            requestPipe.Reader.AdvanceTo(buffer.Start, buffer.End);
            
            var sequenceReader = new SequenceReader<byte>(buffer);

            while (sequenceReader.TryReadTo(out ReadOnlySequence<byte> line, RequestSymbolsAsBytes.NewLine,
                       false))
            {
                if (!TryParseLine(line, out var error))
                    return new(error);
            }

            if (result.IsCompleted)
                break;
        }

        await requestPipe.Reader.CompleteAsync();

        return new(_httpContextBuilder.Build());
    }

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
            case ParsingState.BodyParsing:
                break;
        }

        error = null;
        return true;
    }

    [SkipLocalsInit]
    private Result ParseHeader(ReadOnlySequence<byte> line)
    {
        var reader = new SequenceReader<byte>(line);

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return new Result(new Error(3, "Header has wrong format"));

        reader.Advance(1); // Need to skip space

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerValueSequence, RequestSymbolsAsBytes.CarriageReturnSymbol, true))
            return new Result(new Error(3, "Header has wrong format"));

        _httpContextBuilder.AddHeader(GetReadOnlyMemoryFromSequence(headerTitleSequence), GetReadOnlyMemoryFromSequence(headerValueSequence));

        return new();
    }

    /// <summary>
    /// Extracts request method, route and protocol version from request line
    /// </summary>
    /// <param name="line">Request line of the entire request</param>
    /// <param name="method">Method of the entire request</param>
    /// <param name="route">Route of the entire request</param>
    /// <param name="protocolVersion">Http protocol version of the entire request</param>
    /// <returns>Error if operation was not success, otherwise null</returns>
    private Result ParseRequestLine(ReadOnlySequence<byte> line)
    {
        var reader = new SequenceReader<byte>(line);

        if (reader.TryReadTo(out ReadOnlySequence<byte> methodSequence, RequestSymbolsAsBytes.Space, false)
            && reader.TryReadTo(out ReadOnlySequence<byte> routeSequence, RequestSymbolsAsBytes.Space, false)
            && reader.TryReadTo(out ReadOnlySequence<byte> protocolVersionSequence, RequestSymbolsAsBytes.NewRequestLine,
                false))
        {
            _httpContextBuilder.WithMethod(GetReadOnlyMemoryFromSequence(methodSequence));
            _httpContextBuilder.WithRoute(GetReadOnlyMemoryFromSequence(routeSequence));
            _httpContextBuilder.WithProtocolVersion(GetReadOnlyMemoryFromSequence(protocolVersionSequence));

            return new();
        }

        return new(new Error(2, "Request line has wrong format")); // Error can be instantiated only once in constant class and reused
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