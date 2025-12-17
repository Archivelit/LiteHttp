namespace LiteHttp.Routing;

public interface IEndpointProvider
{
    public Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method);
}