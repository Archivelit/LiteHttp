namespace LiteHttp.Routing;

internal sealed class FrozenEndpointProvider : IEndpointProvider
{
    private readonly FrozenDictionary<Endpoint, Func<IActionResult>> _frozenEndpoints;

    public FrozenEndpointProvider(Dictionary<Endpoint, Func<IActionResult>> endpoints) =>
        _frozenEndpoints = endpoints.ToFrozenDictionary(EndpointComparer.Instance);

    public Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method) =>
        _frozenEndpoints.GetValueOrDefault(new(path, method));
}

internal sealed class EndpointProvider(Dictionary<Endpoint, Func<IActionResult>> endpoints) : IEndpointProvider
{
    public EndpointProvider() : this(new Dictionary<Endpoint, Func<IActionResult>>(EndpointComparer.Instance)) { }

    public Func<IActionResult>? GetEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method) =>
        endpoints.GetValueOrDefault(new(path, method));

    public void AddEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method, Func<IActionResult> action) =>
        endpoints.TryAdd(new(path, method), action);

    public FrozenEndpointProvider Freeze() =>
        new(endpoints);
}