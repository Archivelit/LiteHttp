namespace LiteHttp.Extensions;

public static class ResponseCodeExtensions
{
    public static ReadOnlyMemory<byte> AsByteString(this ResponseCode responseCode) => responseCode switch
    {
        ResponseCode.Ok => ResponseCodesAsBytes.Ok,
        ResponseCode.BadRequest => ResponseCodesAsBytes.BadRequest,
        ResponseCode.NotFound => ResponseCodesAsBytes.NotFound,
        ResponseCode.InternalServerError => ResponseCodesAsBytes.InternalServerError,
        _ => throw new FormatException("Unknown response code")
    };
}