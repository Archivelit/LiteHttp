namespace LiteHttp.Abstractions;

public interface IEndpointProvider
{
    public Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method);
}