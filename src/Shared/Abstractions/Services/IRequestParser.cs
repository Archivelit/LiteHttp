namespace LiteHttp.Abstractions;

public interface IRequestParser
{
    HttpContext Parse(Memory<byte> request);
}