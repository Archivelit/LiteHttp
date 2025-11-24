namespace LiteHttp.Server.Services.Endpoints;

internal sealed class EndpointProviderConfiguration : IEndpointProviderConfiguration
{
    private EndpointContext _endpointContext = new();
    private EndpointProvider _endpointProvider = new();

    public IEndpointContext EndpointContext { get => _endpointContext; }

    public void AddEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(path, method, action);

    public void Freeze() =>
        _endpointContext.SetEndpointProvider(_endpointProvider.Freeze());
}
