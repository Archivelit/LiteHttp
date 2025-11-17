namespace LiteHttp.Abstractions;

public interface IParser
{
    Result<HttpContext> Parse(Memory<byte> request);
}