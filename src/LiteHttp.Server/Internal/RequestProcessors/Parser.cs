namespace LiteHttp.RequestProcessors;

#nullable disable
internal sealed class Parser : IParser
{
    public static readonly Parser Instance = new();

    public Result<HttpContext> Parse(Memory<byte> request)
    {
        var requestParts = SplitRequest(request);

        if (!requestParts.Success)
            return new(requestParts.Exception);

        var firstLine = GetFirstLine(requestParts.Value.Headers);

        var method = GetMethod(firstLine);
        
        if (!method.Success)
            return new(method.Exception);
        
        var route = GetRoute(firstLine);

        if (!route.Success)
            return new (route.Exception);

        var headerSection = requestParts.Value.Headers[(firstLine.Length + RequestSymbolsAsBytes.NewRequestLine.Length)..]; // First line of request does not contain any header

        return new Result<HttpContext>(new HttpContext(method.Value, route.Value, MapHeaders(headerSection).Value, requestParts.Value.Body));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result<Memory<byte>> GetRoute(Memory<byte> firstRequestLine)
    {
        var firstSpaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);
        var lastSpaceIndex = firstRequestLine.Span.LastIndexOf(RequestSymbolsAsBytes.Space);
        
        if (firstSpaceIndex == lastSpaceIndex)
            return new(new ArgumentException("The request has wrong format"));

        return new(firstRequestLine[(firstSpaceIndex+1)..lastSpaceIndex]); // space index + 1 to exclude whitespace and get first symbol of route
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Memory<byte> GetFirstLine(Memory<byte> firstRequestLine) =>
        firstRequestLine[..firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.CarriageReturnSymbol)];

    private Result<Memory<byte>> GetMethod(Memory<byte> firstRequestLine)
    {
        var spaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);

        if (spaceIndex == -1)
            return new(new ArgumentException("The request has wrong format"));
        
        return new(firstRequestLine[..spaceIndex]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result<(Memory<byte> Headers, Memory<byte>? Body)> SplitRequest(Memory<byte> request)
    {
        var splitterIndex = request.Span.IndexOf(RequestSymbolsAsBytes.RequestSplitter);

        if (splitterIndex == -1)
            return new((request, null));
        
        return new((request[..(splitterIndex + RequestSymbolsAsBytes.NewRequestLine.Length)], // NOTE: do not change, it is breaking change. Adding 1 new line symbol for proper header parsing 
            request[(splitterIndex + RequestSymbolsAsBytes.RequestSplitter.Length)..]));
    }
    
    [SkipLocalsInit]
    private Result<Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>> MapHeaders(Memory<byte> headers)
    {
        var headersDictionary = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>(8);

        while (true)
        {
            if (headers.Length < 2) break;

            var eol = headers.Span.IndexOf(RequestSymbolsAsBytes.NewLine);


            if (eol == -1)
            {
                var colonIndex = headers.Span.IndexOf(RequestSymbolsAsBytes.Colon);
             
                if (colonIndex == -1) break;
                headersDictionary.Add(headers[..colonIndex], headers[(colonIndex + 2)..]); // +2 to exclude colon and space,
                return new(headersDictionary);
            }

            var colon = headers.Span[..eol].IndexOf(RequestSymbolsAsBytes.Colon);
            
            if (colon == -1)
                return new(new FormatException("The headers had wrong format"));
            
            Debug.Assert(colon < eol);

            var key = headers[..colon];
            var value = headers[(colon + 2)..(eol - 1)]; // +2 to exclude colon and space, -1 to exclude carriage return (\r) symbol
            
            headersDictionary[key] = value;

            headers = headers[(eol + 1)..]; // +1 to exclude eol symbol
        }

        return new(headersDictionary);
    }
}