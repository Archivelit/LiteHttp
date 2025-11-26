namespace LiteHttp.Constants;

public static class RequestSymbolsAsBytes
{
    public static readonly byte CarriageReturnSymbol = (byte)'\r';
    public static readonly byte Space = (byte)' ';
    public static readonly byte NewLine = (byte)'\n';
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
    public static readonly byte[] Ok =
        " 200 OK\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Created =
        " 201 Created\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Accepted =
        " 202 Accepted\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NoContent =
        " 204 No Content\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MultipleChoices =
        " 300 Multiple Choices\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotModified =
        " 304 Not Modified\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] BadRequest =
        " 400 Bad Request\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Unauthorized =
        " 401 Unauthorized\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Forbidden =
        " 403 Forbidden\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotFound =
        " 404 Not Found\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] MethodNotAllowed =
        " 405 Method Not Allowed\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] RequestTimeout =
        " 408 Request Timeout\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] Conflict =
        " 409 Conflict\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ContentTooLarge =
        " 413 Content Too Large\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] TooManyRequests =
        " 429 Too Many Requests\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] InternalServerError =
        " 500 Internal Server Error\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotImplemented =
        " 501 Not Implemented\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] ServiceUnavailable =
        " 503 Service Unavailable\r\n".AsMemoryByteArray().ToArray();
}