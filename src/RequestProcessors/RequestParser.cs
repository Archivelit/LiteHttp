namespace LiteHttp.RequestProcessors;

public class RequestParser : IRequestParser
{
    public HttpContext Parse(string request)
    {
        var requestParts = SplitRequest(request);
        if (requestParts.Length < 2)
            throw new ArgumentException("The request has unsupported or incorrect format");

        var firstRequestLine = GetFirstLine(requestParts[0]);

        var method = GetMethod(firstRequestLine);
        var path = GetPath(firstRequestLine);
        
        var headers = requestParts[0][firstRequestLine.Length..]; // First line of request does not contain any header
        var body = requestParts[1];

        return new(method, path, MapHeaders(headers), body);
    }

    private string GetPath(string firstRequestLine)
    {
        var firstSpaceIndex = firstRequestLine.IndexOf(' ', StringComparison.Ordinal);
        var lastSpaceIndex = firstRequestLine.LastIndexOf(' ');

        return firstRequestLine[(firstSpaceIndex+1)..(lastSpaceIndex-1)]; // space index +- 1 to get first/last symbol
    }

    private string GetFirstLine(string request) =>
        request[..request.IndexOf('\r', StringComparison.Ordinal)];

    private string GetMethod(string request) =>
        request[..request.IndexOf(' ', StringComparison.Ordinal)];

    private string[] SplitRequest(string request) =>
        request.Split("\r\n\r\n", 2);

    [SkipLocalsInit]
    private Dictionary<string, string> MapHeaders(string headers)
    {
        var headerLines = headers.Split('\n');
        var headersDictionary = new Dictionary<string, string>(headerLines.Length, StringComparer.OrdinalIgnoreCase);
        
        for (var i = 0; i < headerLines.Length; i++)
        {
            if (string.IsNullOrEmpty(headerLines[i].Trim()))
                continue;
            
            var colonIndex = headerLines[i].IndexOf(':');

            if (colonIndex == -1)
                throw new FormatException("Headers must contain ':' symbol");

            var key = headerLines[i][(colonIndex+1)..].Trim(); 
            var value= headerLines[i][..colonIndex].Trim();

            headersDictionary.Add(key, value);
        }

        return headersDictionary;
    }
}