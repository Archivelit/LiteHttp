namespace LiteHttp.RequestProcessors;

public class RequestParser : IRequestParser
{
    public HttpContext Parse(string request)
    {
        var splitedRequest = SplitRequest(request);
        if (splitedRequest.Length < 2)
            throw new ArgumentException("The request has unsupported or incorrect format");

        var firstRequestLine = GetFirstLine(splitedRequest[0]);

        var method = GetMethod(firstRequestLine);
        var path = GetPath(firstRequestLine);
        
        var headers = splitedRequest[0];
        var body = splitedRequest[1];

        return new(method, path, MapHeaders(headers), body);
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
