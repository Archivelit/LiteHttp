namespace LiteHttp.Abstractions;

public interface IParser
{
    HttpContext Parse(Memory<byte> request);
}