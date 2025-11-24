namespace LiteHttp.Abstractions;

public interface IEndpointProvider
{
    Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method);
}