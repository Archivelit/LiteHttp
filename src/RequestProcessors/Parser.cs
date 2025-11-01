namespace LiteHttp.RequestProcessors;

public class Parser : IParser
{
    public HttpContext Parse(Memory<byte> request)
    {
        var requestParts = SplitRequest(request);
        
        var firstLine = GetFirstLine(requestParts.Headers);

        var method = GetMethod(firstLine);
        var path = GetPath(firstLine);

        var headers = requestParts.Headers[firstLine.Length..]; // First line of request does not contain any header

        return new(method, path, MapHeaders(headers), requestParts.Body);
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
        
        return (request[..splitterIndex], request[splitterIndex..]);
    }
    
    [SkipLocalsInit]
    private Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> MapHeaders(Memory<byte> headers)
    {
        var headersDictionary = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>(8);
        
        while(true)
        {
            var trimmed = headers.Trim((byte)' ');

            var eol = trimmed.Span.IndexOf(RequestSymbolsAsBytes.NewLine);
            if (eol == -1)
                break;

            var colon = trimmed.Span[..eol].IndexOf(RequestSymbolsAsBytes.Colon);
            if (colon == -1)
                break;
            
            Debug.Assert(colon > eol);

            var key = trimmed[..colon];
            var value = trimmed[(colon + 2)..eol]; // 2 symbols are colon and space between value and colon
            
            headersDictionary.Add(key, value);

            headers = trimmed[eol..];
        }

        return headersDictionary;
    }
    
    [Obsolete("In new versions server works with bytes directly, so working strings are deprecated. " +
              "I decided to let it to reuse logic in new MapHeaders realisation")]
    private (string key, string value)? PerformOnString(ReadOnlySpan<byte> headerLine)
    {
        var lineAsString = Encoding.UTF8.GetString(headerLine);
            
        if (string.IsNullOrEmpty(lineAsString.Trim()))
            return null;
            
        var colonIndex = lineAsString.IndexOf(':');

        if (colonIndex == -1)
            throw new FormatException("Headers must contain ':' symbol");

        var key = lineAsString[..colonIndex].Trim(); 
        var value = lineAsString[(colonIndex + 1)..].Trim();

        return (key, value);
    }
}