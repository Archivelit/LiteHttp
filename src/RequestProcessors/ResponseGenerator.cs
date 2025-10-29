namespace LiteHttp.RequestProcessors;

public class ResponseGenerator : IResponseGenerator
{
    private readonly string _newLine = "\r\n";

    public int Port { get; set; }
    public string Address
    {
        get;

        set
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            
            field = value;
        }
    }
    
    private string _host => $"{Address}:{Port}";

    public ResponseGenerator()
    {
        Address = AddressConstants.IPV4_LOOPBACK.ToString();
        Port = AddressConstants.DEFAULT_SERVER_PORT;
    }

    [SkipLocalsInit]
    public string Generate(IActionResult actionResult, string httpVersion, string? responseBody = null)
    {
        var responseBuilder = new StringBuilder(64);

        responseBuilder
            .Append(httpVersion)
            .Append(actionResult.ResponseCode.AsString())
            .Append(GenerateHeaders(responseBody ?? string.Empty, httpVersion))
            .Append(_newLine)
            .Append(_newLine)
            .Append(responseBody);

        return responseBuilder.ToString();
    }

    [SkipLocalsInit]
    private string GenerateHeaders(string body, string httpVersion)
    {
        var headersBuilder = new StringBuilder(64);

        if (httpVersion == HttpVersions.HTTP_1_1)
        {
            headersBuilder.Append($"Host: {_host}{_newLine}");
        }

        headersBuilder.Append($"Content-Type: text/plain{_newLine}");

        if (body.Length != 0)
        {
            headersBuilder.Append($"Content-Length: {body.Length}{_newLine}");
        }

        return headersBuilder.ToString();
    }
}