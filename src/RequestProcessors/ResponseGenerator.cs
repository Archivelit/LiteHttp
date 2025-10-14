namespace LiteHttp.RequestProcessors;

public class ResponseGenerator : IResponseGenerator
{
    private readonly string _newLine = "\r\n";

    [SkipLocalsInit]
    public string Generate(IActionResult actionResult, string httpVersion, string responseBody)
    {
        var resultBuilder = new StringBuilder(64);

        resultBuilder.Append(httpVersion);
        resultBuilder.Append(
            actionResult.ResponseCode switch
            {
                Enums.ResponseCode.Ok => $" 200 OK{_newLine}",
                Enums.ResponseCode.BadRequest => $" 400 Bad Request{_newLine}",
                Enums.ResponseCode.NotFound => $" 404 Not Found{_newLine}",
                Enums.ResponseCode.InternalServerError => $" 500 Internal Server Error{_newLine}",
                _ => throw new FormatException("Unknown response code")
            });

        // TODO: Implement request headers generation
        // resultBuilder.Append(GetHeaders());

        resultBuilder.Append(_newLine);
        resultBuilder.Append(_newLine);

        resultBuilder.Append(responseBody);

        return resultBuilder.ToString();
    }
}