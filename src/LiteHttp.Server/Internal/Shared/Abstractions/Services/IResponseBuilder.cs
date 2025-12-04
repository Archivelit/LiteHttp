namespace LiteHttp.Abstractions;

public interface IResponseBuilder
{
    public Result<ReadOnlyMemory<byte>> Build(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null);
}