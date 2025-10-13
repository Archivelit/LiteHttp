namespace LiteHttp.RequestProcessors;

public sealed class RequestParser : IRequestParser
{
    public HttpContext? Parse(string request)
    {
        var splitedRequest = GetSplitedRequest(request);
        if (splitedRequest.Length < 2)
            throw new ArgumentException("The request has unsupported or incorrect format");

        var headers = splitedRequest[0];
        var body = splitedRequest[1];

        return new(ParseHeadersToDictionary(headers), body);
    }

    private string[] GetSplitedRequest(string request) =>
        request.Split("\r\n\r\n", 2);

    [SkipLocalsInit]
    private Dictionary<string, string> ParseHeadersToDictionary(string headers)
    {
        var splitedHeaders = headers.Split("\r\n");
        var headersDictionary = new Dictionary<string, string>(splitedHeaders.Length, StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < splitedHeaders.Length; i++)
        {
            var index = splitedHeaders[i].IndexOf(':');

            if (index == -1)
                throw new FormatException("Headers must contain ':' symbol");

            var key = splitedHeaders[i][..index].Trim();
            var value = splitedHeaders[i][(index + 1)..].Trim();

            headersDictionary.Add(key, value);
        }

        return headersDictionary;
    }
}
