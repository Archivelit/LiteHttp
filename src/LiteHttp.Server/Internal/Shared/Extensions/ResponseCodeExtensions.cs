namespace LiteHttp.Extensions;

public static class ResponseCodeExtensions
{
    public static ReadOnlyMemory<byte> AsByteString(this ResponseCode responseCode) => responseCode switch
    {
        ResponseCode.Ok => ResponseCodesAsBytes.Ok,
        ResponseCode.Created => ResponseCodesAsBytes.Created,
        ResponseCode.Accepted => ResponseCodesAsBytes.Accepted,
        ResponseCode.NoContent => ResponseCodesAsBytes.NoContent,
        ResponseCode.MultipleChoices => ResponseCodesAsBytes.MultipleChoices,
        ResponseCode.NotModified => ResponseCodesAsBytes.NotModified,
        ResponseCode.BadRequest => ResponseCodesAsBytes.BadRequest,
        ResponseCode.Unauthorized => ResponseCodesAsBytes.Unauthorized,
        ResponseCode.Forbidden => ResponseCodesAsBytes.Forbidden,
        ResponseCode.NotFound => ResponseCodesAsBytes.NotFound,
        ResponseCode.MethodNotAllowed => ResponseCodesAsBytes.MethodNotAllowed,
        ResponseCode.RequestTimeout => ResponseCodesAsBytes.RequestTimeout,
        ResponseCode.Conflict => ResponseCodesAsBytes.Conflict,
        ResponseCode.ContentTooLarge => ResponseCodesAsBytes.ContentTooLarge,
        ResponseCode.TooManyRequests => ResponseCodesAsBytes.TooManyRequests,
        ResponseCode.InternalServerError => ResponseCodesAsBytes.InternalServerError,
        ResponseCode.NotImplemented => ResponseCodesAsBytes.NotImplemented,
        ResponseCode.ServiceUnavailable => ResponseCodesAsBytes.ServiceUnavailable,
        _ => throw new FormatException("Unknown response code")
    };
}