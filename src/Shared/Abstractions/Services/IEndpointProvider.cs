namespace LiteHttp.Abstractions;

public interface IEndpointProvider
{
    Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method);
    void AddEndpoint(ReadOnlyMemory<byte> spath, ReadOnlyMemory<byte> method, Func<IActionResult> action);
}