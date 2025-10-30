namespace LiteHttp.RequestProcessors;

#pragma warning disable CS8618
public class ResponseGenerator : IResponseGenerator
{
    private readonly string _newLine = "\r\n";

    public int Port
    {
        get;

        set
        {
            field = value;

            UpdateHost();
        }
    }
    public string Address
    {
        get;

        set
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            
            field = value;

            UpdateHost();
        }
    }
    
    private string _host;

    public ResponseGenerator()
    {
        Address = AddressConstants.IPV4_LOOPBACK.ToString();
        Port = AddressConstants.DEFAULT_SERVER_PORT;
    }

    [SkipLocalsInit]
    public string Generate(IActionResult actionResult, string? responseBody = null)
    {
        var bodyLength = responseBody?.Length ?? 0;

        var responseBuilder = new StringBuilder(128 + bodyLength);

        responseBuilder
            .Append(HttpVersions.HTTP_1_1)
            .Append(actionResult.ResponseCode.AsString());
        
        GenerateHeaders(responseBuilder, responseBody ?? string.Empty);
        
        if (!string.IsNullOrEmpty(responseBody))
            responseBuilder
                .Append(_newLine)
                .Append(responseBody);

        return responseBuilder.ToString();
    }


    [SkipLocalsInit]
    private StringBuilder GenerateHeaders(StringBuilder responseBuilder, string body)
    {
        responseBuilder
            .Append($"Host: {_host}{_newLine}")
            .Append($"Content-Type: text/plain{_newLine}");

        if (!string.IsNullOrEmpty(body))
        {
            responseBuilder.Append($"Content-Length: {body.Length}{_newLine}");
        }

        responseBuilder.Append(_newLine);

        return responseBuilder;
    }

    private void UpdateHost() => _host = $"{Address}:{Port}";
}