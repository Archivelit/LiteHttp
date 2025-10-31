namespace LiteHttp.Constants;

public static class RequestSymbolsAsBytes
{
    public static readonly byte CarriageReturnSymbol = (byte)'\r';
    public static readonly byte Space = (byte)' ';
    public static readonly byte NewLine = (byte)'\n';
    public static readonly byte Colon = (byte)':';
    public static readonly byte[] NewRequestLine = [ CarriageReturnSymbol, NewLine ];
    public static readonly byte[] RequestSplitter = [ CarriageReturnSymbol, NewLine , CarriageReturnSymbol, NewLine  ];
}

public static class RequestMethodsAsBytes
{
    public static readonly byte[] Get = ByteParseHelper.StringAsBytes(RequestMethods.Get);
    public static readonly byte[] Post = ByteParseHelper.StringAsBytes(RequestMethods.Post);
    public static readonly byte[] Put = ByteParseHelper.StringAsBytes(RequestMethods.Put);
    public static readonly byte[] Patch = ByteParseHelper.StringAsBytes(RequestMethods.Patch);
    public static readonly byte[] Delete = ByteParseHelper.StringAsBytes(RequestMethods.Delete);
}

public static class HttpVersionsAsBytes
{
    public static readonly byte[] Http_1_1 = ByteParseHelper.StringAsBytes(HttpVersions.HTTP_1_1);
}

public static class HeadersAsBytes
{
    public static readonly byte[] Host = ByteParseHelper.StringAsBytes("Host: ");
    public static readonly byte[] ContentType = ByteParseHelper.StringAsBytes("Content-Type: ");
    public static readonly byte[] ContentLength = ByteParseHelper.StringAsBytes("Content-Length: ");
}

public static class HeaderValuesAsBytes
{
    public static readonly byte[] ContentTextPlain = ByteParseHelper.StringAsBytes("text/plain\r\n");
}

public static class ResponseCodesAsBytes
{
    public static readonly byte[] Ok =
        ByteParseHelper.StringAsBytes(" 200 OK\r\n");

    public static readonly byte[] BadRequest =
        ByteParseHelper.StringAsBytes(" 400 Bad Request\r\n");

    public static readonly byte[] NotFound =
        ByteParseHelper.StringAsBytes(" 404 Not Found\r\n");

    public static readonly byte[] InternalServerError =
        ByteParseHelper.StringAsBytes(" 500 Internal Server Error\r\n");
}


internal static class ByteParseHelper
{
    public static byte[] StringAsBytes(string s) => Encoding.UTF8.GetBytes(s);
}