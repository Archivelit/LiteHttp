namespace LiteHttp.Abstractions;

public interface IRequestParser
{
    HttpContext? Parse(string request);
}