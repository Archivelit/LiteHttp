namespace LiteHttp.Constants;

public static class RequestSymbolsAsBytes
{
    public static readonly byte CarriageReturnSymbol = (byte)'\r';
    public static readonly byte Space = (byte)' ';
    public static readonly byte LineFeed = (byte)'\n';
    public static readonly byte Colon = (byte)':';

    public static readonly byte[] NewRequestLine =
        "\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] RequestSplitter =
        "\r\n\r\n".AsMemoryByteArray().ToArray();
}

public static class RequestMethodsAsBytes
{
    public static readonly byte[] Get =
        RequestMethods.Get.AsMemoryByteArray().ToArray();

    public static readonly byte[] Post =
        RequestMethods.Post.AsMemoryByteArray().ToArray();

    public static readonly byte[] Put =
        RequestMethods.Put.AsMemoryByteArray().ToArray();

    public static readonly byte[] Patch =
        RequestMethods.Patch.AsMemoryByteArray().ToArray();

    public static readonly byte[] Delete =
        RequestMethods.Delete.AsMemoryByteArray().ToArray();
}

public static class HttpVersionsAsBytes
{
    public static readonly byte[] Http11 =
        HttpVersions.HTTP_1_1.AsMemoryByteArray().ToArray();
}

public static class HeadersAsBytes
{
    public static readonly byte[] Host =
        "Host: ".AsMemoryByteArray().ToArray();

    public static readonly byte[] ContentType =
        "Content-Type: ".AsMemoryByteArray().ToArray();

    public static readonly byte[] ContentLength =
        "Content-Length: ".AsMemoryByteArray().ToArray();
}

public static class HeaderValuesAsBytes
{
    public static readonly byte[] ContentTextPlain =
        "text/plain\r\n".AsMemoryByteArray().ToArray();
}

public static class ResponseCodesAsBytes
{
    public static readonly byte[] Continue =
        " 100 Continue\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] SwitchingProtocols =
        " 101 Switching Protocols\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Processing =
        " 102 Processing\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] EarlyHints =
        " 103 Early Hints\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Ok =
        " 200 OK\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Created =
        " 201 Created\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Accepted =
        " 202 Accepted\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NoContent =
        " 204 No Content\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ResetContent =
        " 205 Reset Content\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] PartialContent =
        " 206 Partial Content\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MultiStatus =
        " 207 Multi Status\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] AlreadyReported =
        " 208 Already Reported\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ImUsed =
        " 226 Im Used\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MultipleChoices =
        " 300 Multiple Choices\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MovedPermanently =
        " 301 Moved Permanently\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Found =
        " 302 Found\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] SeeOther =
        " 303 See Other\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotModified =
        " 304 Not Modified\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UseProxy =
        " 305 Use Proxy\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] SwitchProxy =
        " 306 Switch Proxy\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] TemporaryRedirect =
        " 307 Temporary Redirect\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] PermanentRedirect =
        " 308 Permanent Redirect\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] BadRequest =
        " 400 Bad Request\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Unauthorized =
        " 401 Unauthorized\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] PaymentRequired =
        " 402 Payment Required\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Forbidden =
        " 403 Forbidden\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotFound =
        " 404 Not Found\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MethodNotAllowed =
        " 405 Method Not Allowed\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotAcceptable =
        " 406 Not Acceptable\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ProxyAuthenticationRequired =
        " 407 Proxy Authentication Required\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] RequestTimeout =
        " 408 Request Timeout\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Conflict =
        " 409 Conflict\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Gone =
        " 410 Gone\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] LengthRequired =
        " 411 Length Required\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] PreconditionFailed =
        " 412 Precondition Failed\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ContentTooLarge =
        " 413 Content Too Large\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UriTooLong =
        " 414 URI Too Long\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UnsupportedMediaType =
        " 415 Unsupported Media Type\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] RangeNotSatisfiable =
        " 416 Range Not Satisfiable\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ExpectationFailed =
        " 417 Expectation Failed\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ImATeapot =
        " 418 I'm a teapot\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MisdirectedRequest =
        " 421 Misdirected Request\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UnprocessableEntity =
        " 422 Unprocessable Entity\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Locked =
        " 423 Locked\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] FailedDependency =
        " 424 Failed Dependency\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] TooEarly =
        " 425 Too Early\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UpgradeRequired =
        " 426 Upgrade Required\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] PreconditionRequired =
        " 428 Precondition Required\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] TooManyRequests =
        " 429 Too Many Requests\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] RequestHeaderFieldsTooLarge =
        " 431 Request Header Fields Too Large\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] UnavailableForLegalReasons =
        " 451 Unavailable For Legal Reasons\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] InternalServerError =
        " 500 Internal Server Error\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotImplemented =
        " 501 Not Implemented\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] BadGateway =
        " 502 Bad Gateway\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ServiceUnavailable =
        " 503 Service Unavailable\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] GatewayTimeout =
        " 504 Gateway Timeout\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] HttpVersionNotSupported =
        " 505 HTTP Version Not Supported\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] VariantAlsoNegotiates =
        " 506 Variant Also Negotiates\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] InsufficientStorage =
        " 507 Insufficient Storage\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] LoopDetected =
        " 508 Loop Detected\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotExtended =
        " 510 Not Extended\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NetworkAuthenticationRequired =
        " 511 Network Authentication Required\r\n".AsMemoryByteArray().ToArray();
    
    public static ReadOnlyMemory<byte> AsMemoryByteArray(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return ReadOnlyMemory<byte>.Empty;

        var bytes = Encoding.UTF8.GetBytes(s);
        return new(bytes);
    }
}

