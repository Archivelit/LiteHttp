namespace LiteHttp.RequestProcessors.Pipeline;

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
            
        while (true)
        {
            var result = await requestPipe.Reader.ReadAsync();
            
            var buffer = result.Buffer;
            
            requestPipe.Reader.AdvanceTo(buffer.Start, buffer.End);
            
            var sequenceReader = new SequenceReader<byte>(buffer);

            while (sequenceReader.TryReadTo(out ReadOnlySequence<byte> line, RequestSymbolsAsBytes.NewLine,
                       false))
            {
                var error = ParseLine(line);
                if (error is not null)
                    return new Result<HttpContext>(error.Value);
            }

            if (result.IsCompleted)
                break;
        }

        await requestPipe.Reader.CompleteAsync();

        throw new NotImplementedException();

        // Old Parser implementation (using Memory<byte> instead of Pipe)

        /*var requestParts = SplitRequest(requestPipe);

        var firstLine = GetFirstLine(requestParts.Headers);

        var method = GetMethod(firstLine);

        if (!method.Success)
            return new(method.Error.Value);

        var route = GetRoute(firstLine);

        if (!route.Success)
            return new(route.Error.Value);

        var headerSection = requestParts.Headers[(firstLine.Length + RequestSymbolsAsBytes.NewRequestLine.Length)..]; // First line of request does not contain any header

        var headers = MapHeaders(headerSection);

        return !headers.Success
            ? new(headers.Error.Value)
            : new Result<HttpContext>(new HttpContext(method.Value, route.Value, headers.Value, requestParts.Body));*/
    }

    private Error? ParseLine(ReadOnlySequence<byte> sequence)
    {
        switch (_parsingState)
        {
            case ParsingState.RequestLineParsing:
                var error = ParseRequestLine(sequence, out var method, out var route,
                    out var protocolVersion);

                if (error is not null)
                    return error;

                _httpContextBuilder.WithMethod(method);
                _httpContextBuilder.WithRoute(route);
                _httpContextBuilder.WithProtocolVersion(protocolVersion);

                break;
            case ParsingState.HeadersParsing:
                break;
            case ParsingState.BodyParsing:
                break;
        }

        return null;
    }
    
    /// <summary>
    /// Extracts request method, route and protocol version from request line
    /// </summary>
    /// <param name="line">Request line of the entire request</param>
    /// <param name="method">Method of the entire request</param>
    /// <param name="route">Route of the entire request</param>
    /// <param name="protocolVersion">Http protocol version of the entire request</param>
    /// <returns>Error if operation was not success, otherwise null</returns>
    private Error? ParseRequestLine(ReadOnlySequence<byte> line, out ReadOnlyMemory<byte> method, out ReadOnlyMemory<byte> route,
        out ReadOnlyMemory<byte> protocolVersion)
    {
        method = ReadOnlyMemory<byte>.Empty;
        route = ReadOnlyMemory<byte>.Empty;
        protocolVersion = ReadOnlyMemory<byte>.Empty;
        
        var reader = new SequenceReader<byte>(line);
        
        if (!reader.TryReadTo(out ReadOnlySequence<byte> methodSequence, RequestSymbolsAsBytes.Space, false) 
            || !reader.TryReadTo(out ReadOnlySequence<byte> routeSequence, RequestSymbolsAsBytes.Space, false) 
            || !reader.TryReadTo(out ReadOnlySequence<byte> protocolVersionSequence, RequestSymbolsAsBytes.NewRequestLine,
                false))
            return new Error(2, "Request line has wrong format");

        method = new ReadOnlyMemory<byte>(methodSequence.ToArray());
        route = new ReadOnlyMemory<byte>(routeSequence.ToArray());
        protocolVersion = new ReadOnlyMemory<byte>(protocolVersionSequence.ToArray());
        
        return null;
    }

    /// <summary>
    /// Maps the entire request header section on specified headers.
    /// </summary>
    /// <param name="headers">The entire request header section</param>
    /// <returns><see cref="Result{TResult}"/> wrapee with exception or headers dictionary.
    /// The dictionaries key is header title without column</returns>
    [SkipLocalsInit]
    private Result<Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>> MapHeaders(Memory<byte> headers)
    {
        var headersDictionary = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>(8);

        while (headers.Length > 2)
        {
            var eol = headers.Span.IndexOf(RequestSymbolsAsBytes.NewLine);

            if (eol == -1)
            {
                var colonIndex = headers.Span.IndexOf(RequestSymbolsAsBytes.Colon);

                if (colonIndex == -1)
                    return new(headersDictionary);

                headersDictionary.Add(headers[..colonIndex], headers[(colonIndex + 2)..]); // +2 to exclude colon and space,

                return new(headersDictionary);
            }

            var colon = headers.Span[..eol].IndexOf(RequestSymbolsAsBytes.Colon);

            if (colon == -1)
                return new(new Error(ParserErrors.InvalidRequestSyntax, "The headers had wrong format"));

            var key = headers[..colon];
            var value = headers[
                (colon + 2)..(eol - 1)]; // +2 to exclude colon and space, -1 to exclude carriage return (\r) symbol

            headersDictionary[key] = value;
            headers = headers[(eol + 1)..]; // +1 to exclude eol symbol
        }

        return new(headersDictionary);
    }

    private enum ParsingState
    {
        RequestLineParsing,
        HeadersParsing,
        BodyParsing
    }
}