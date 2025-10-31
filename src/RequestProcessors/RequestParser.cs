namespace LiteHttp.RequestProcessors;

public class RequestParser : IRequestParser
{
    public HttpContext Parse(Memory<byte> request)
    {
        var requestParts = SplitRequest(request);
        
        var firstRequestLine = GetFirstLine(requestParts[0]);

        var method = GetMethod(firstRequestLine);
        var path = GetPath(firstRequestLine);
        
        var headers = requestParts[0][firstRequestLine.Length..]; // First line of request does not contain any header
        var body = requestParts[1];

        return new(method, path, MapHeaders(headers), body);
    }

    private Memory<byte> GetPath(Memory<byte> firstRequestLine)
    {
        var firstSpaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);
        var lastSpaceIndex = firstRequestLine.Span.LastIndexOf(RequestSymbolsAsBytes.Space);

        return firstRequestLine[(firstSpaceIndex+1)..lastSpaceIndex]; // space index + 1 to get first symbol of path
    }

    private Memory<byte> GetFirstLine(Memory<byte> request) =>
        request[..request.Span.IndexOf(RequestSymbolsAsBytes.CarriageReturnSymbol)];

    private Memory<byte> GetMethod(Memory<byte> request) =>
        request[..request.Span.IndexOf(RequestSymbolsAsBytes.Space)];

    private Memory<byte>[] SplitRequest(Memory<byte> request)
    {
        var splitterIndex = request.Span.IndexOf(RequestSymbolsAsBytes.RequestSplitter);
        
        if (splitterIndex == -1)
            throw new ArgumentException("The request has unsupported format");
        
        return [ request[..splitterIndex], request[splitterIndex..] ];
    }
    
    [SkipLocalsInit]
    private Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> MapHeaders(Memory<byte> headers)
    {
        var headersDictionary = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>(8);
        
        while(true)
        {
            var eol = headers.Span.IndexOf(RequestSymbolsAsBytes.NewLine);
            if (eol == -1)
                break;

            var colon = headers.Span.IndexOf(RequestSymbolsAsBytes.Colon);
            if (colon == -1)
                continue;
            
            var key = headers[..colon];
            var value = headers[colon..eol];
            
            headersDictionary.Add(key, value);

            headers = headers[eol..];
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