namespace LiteHttp.RequestProcessors;

public sealed class Parser : IParser
{
    public static readonly Parser Instance = new();

    public HttpContext Parse(Memory<byte> request)
    {
        var requestParts = SplitRequest(request);
        
        var firstLine = GetFirstLine(requestParts.Headers);

        var method = GetMethod(firstLine);
        var path = GetPath(firstLine);

        var headerSection = requestParts.Headers[(firstLine.Length + RequestSymbolsAsBytes.NewRequestLine.Length)..]; // First line of request does not contain any header

        return new(method, path, MapHeaders(headerSection), requestParts.Body);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Memory<byte> GetPath(Memory<byte> firstRequestLine)
    {
        var firstSpaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);
        var lastSpaceIndex = firstRequestLine.Span.LastIndexOf(RequestSymbolsAsBytes.Space);

        return firstRequestLine[(firstSpaceIndex+1)..lastSpaceIndex]; // space index + 1 to get first symbol of path
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Memory<byte> GetFirstLine(Memory<byte> request) =>
        request[..request.Span.IndexOf(RequestSymbolsAsBytes.CarriageReturnSymbol)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Memory<byte> GetMethod(Memory<byte> request) =>
        request[..request.Span.IndexOf(RequestSymbolsAsBytes.Space)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (Memory<byte> Headers, Memory<byte>? Body) SplitRequest(Memory<byte> request)
    {
        var splitterIndex = request.Span.IndexOf(RequestSymbolsAsBytes.RequestSplitter);

        if (splitterIndex == -1)
            return (request, null);
        
        return (request[..(splitterIndex + RequestSymbolsAsBytes.NewRequestLine.Length)], // NOTE: do not change, it is breaking change. Adding 1 new line symbol for proper header parsing 
            request[(splitterIndex + RequestSymbolsAsBytes.RequestSplitter.Length)..]);
    }
    
    [SkipLocalsInit]
    private Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> MapHeaders(Memory<byte> headers)
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
                return headersDictionary;
            }

            var colon = headers.Span[..eol].IndexOf(RequestSymbolsAsBytes.Colon);
            
            if (colon == -1)
                throw new FormatException("The headers had wrong format");
            
            Debug.Assert(colon < eol);

            var key = headers[..colon];
            var value = headers[(colon + 2)..(eol - 1)]; // +2 to exclude colon and space, -1 to exclude carriage return (\r) symbol
            
            headersDictionary[key] = value;

            headers = headers[(eol + 1)..]; // +1 to exclude eol symbol
        }

        return headersDictionary;
    }
}