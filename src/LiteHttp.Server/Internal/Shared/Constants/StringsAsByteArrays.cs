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

    public static readonly byte[] BadRequest =
        " 400 Bad Request\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] NotFound =
        " 404 Not Found\r\n".AsMemoryByteArray().ToArray();

    public static readonly byte[] InternalServerError =
        " 500 Internal Server Error\r\n".AsMemoryByteArray().ToArray();
}