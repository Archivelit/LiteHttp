namespace LiteHttp.Server.Services.Endpoints;

internal class EndpointContext : IEndpointContext
{
    private IEndpointProvider? _endpointProvider;

    public IEndpointProvider EndpointProvider 
    { 
        get => _endpointProvider 
            ?? throw new InvalidOperationException("Endpoint provider was not bound"); 
    }

    internal void SetEndpointProvider(IEndpointProvider endpointProvider) =>
        _endpointProvider = endpointProvider;
}
