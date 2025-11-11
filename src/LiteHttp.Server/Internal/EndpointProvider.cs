namespace LiteHttp.Server;

internal sealed class EndpointProvider(
    ConcurrentDictionary<Endpoint, Func<IActionResult>> endpoints
    ) : IEndpointProvider
{
    public EndpointProvider() : this(new(EndpointComparer.Instance)) { }

    public Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method) =>
        endpoints.GetValueOrDefault(new(path, method));

    public void AddEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method, Func<IActionResult> action) =>
        endpoints.TryAdd(new(path, method), action);
}