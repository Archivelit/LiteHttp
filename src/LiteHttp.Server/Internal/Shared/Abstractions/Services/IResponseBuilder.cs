namespace LiteHttp.Abstractions;

public interface IResponseBuilder
{
    public ReadOnlyMemory<byte> Build(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null);
}