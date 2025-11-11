namespace LiteHttp.Abstractions;

public interface IResponseBuilder
{
    ReadOnlyMemory<byte> Build(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null);
}