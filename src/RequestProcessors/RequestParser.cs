namespace LiteHttp.RequestProcessors;

public sealed class RequestParser : IRequestParser
{
    public HttpContext Parse(string request)
    {
        var requestParts = SplitRequest(request);
        
        var headers = MapHeaders(requestParts[0]);
        var body = requestParts[1];
        
        var firstLine = GetFirstLine(requestParts[0]);
        var method = GetMethod(firstLine);
        var path = GetPath(firstLine);
        
        return new HttpContext(method, path, headers, body);
    }

    private string GetPath(string firstRequestLine)
    {
        var firstTabIndex = firstRequestLine.IndexOf(' ', StringComparison.Ordinal);
        var lastTabIndex = firstRequestLine.LastIndexOf(' ');

        return firstRequestLine[firstTabIndex..lastTabIndex];
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
        var headerLines = headers.Split("\r\n");
        var headersDictionary = new Dictionary<string, string>(headerLines.Length, StringComparer.OrdinalIgnoreCase);
        
        for (var i = 0; i < headerLines.Length; i++)
        {
            var index = headerLines[i].IndexOf(':');

            if (index == -1)
                throw new FormatException("Headers must contain ':' symbol");

            var key = headerLines[i][..index].Trim();
            var value= headerLines[i][(index+1)..].Trim();
            
            headersDictionary.Add(key, value);
        }

        return headersDictionary;
    }
}
