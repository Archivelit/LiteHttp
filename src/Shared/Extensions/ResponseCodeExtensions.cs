namespace LiteHttp.Extensions;

public static class ResponseCodeExtensions
{
    private const string _newLine = "\r\n";

    public static string AsString(this ResponseCode responseCode) => responseCode switch
    {
        ResponseCode.Ok => $" 200 OK{_newLine}",
        ResponseCode.BadRequest => $" 400 Bad Request{_newLine}",
        ResponseCode.NotFound => $" 404 Not Found{_newLine}",
        ResponseCode.InternalServerError => $" 500 Internal Server Error{_newLine}",
        _ => throw new FormatException("Unknown response code")
    };
}