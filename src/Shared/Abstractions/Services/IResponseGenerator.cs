namespace LiteHttp.Abstractions;

public interface IResponseGenerator
{
    ReadOnlyMemory<byte> Generate(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null);
}