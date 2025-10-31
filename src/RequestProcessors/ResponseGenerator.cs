namespace LiteHttp.RequestProcessors;

#pragma warning disable CS8618
public class ResponseGenerator : IResponseGenerator
{
    private readonly string _newLine = "\r\n";
    private readonly StringBuilder _responseBuilder = new StringBuilder(1024);
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
        _responseBuilder.Clear();
        
        _responseBuilder
            .Append(HttpVersions.HTTP_1_1)
            .Append(actionResult.ResponseCode.AsString());
        
        GenerateHeaders(responseBody ?? string.Empty);
        
        if (!string.IsNullOrEmpty(responseBody))
            _responseBuilder
                .Append(_newLine)
                .Append(responseBody);

        return _responseBuilder.ToString();
    }


    [SkipLocalsInit]
    private StringBuilder GenerateHeaders(string body)
    {
        _responseBuilder
            .Append($"Host: {_host}{_newLine}")
            .Append($"Content-Type: text/plain{_newLine}");

        if (!string.IsNullOrEmpty(body))
        {
            _responseBuilder.Append($"Content-Length: {body.Length}{_newLine}");
        }

        _responseBuilder.Append(_newLine);

        return _responseBuilder;
    }

    private void UpdateHost() => _host = $"{Address}:{Port}";
}