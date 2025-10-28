using LiteHttp.Constants;
using System.Net;

namespace LiteHttp.RequestProcessors;

public class ResponseGenerator : IResponseGenerator
{
    private readonly string _newLine = "\r\n";
    private string Host { get; set; }

    public ResponseGenerator(string address, int port) =>
        Host = $"{address}:{port}";       


    [SkipLocalsInit]
    public string Generate(IActionResult actionResult, string httpVersion, string? responseBody = null)
    {
        var responseBuilder = new StringBuilder(64);

        responseBuilder
            .Append(httpVersion)
            .Append(
                actionResult.ResponseCode switch
                {
                    Enums.ResponseCode.Ok => $" 200 OK{_newLine}",
                    Enums.ResponseCode.BadRequest => $" 400 Bad Request{_newLine}",
                    Enums.ResponseCode.NotFound => $" 404 Not Found{_newLine}",
                    Enums.ResponseCode.InternalServerError => $" 500 Internal Server Error{_newLine}",
                    _ => throw new FormatException("Unknown response code")
                })
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
            headersBuilder.Append($"Host: {Host}{_newLine}");
        }

        headersBuilder.Append($"Content-Type: text/plain{_newLine}");

        if (body.Length != 0)
        {
            headersBuilder.Append($"Content-Length: {body.Length}{_newLine}");
        }

        return headersBuilder.ToString();
    }
}