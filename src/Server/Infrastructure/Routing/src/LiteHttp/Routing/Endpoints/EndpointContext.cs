namespace LiteHttp.Routing;

internal class EndpointContext : IEndpointContext
{
    public IEndpointProvider EndpointProvider
    {
        get => field
            ?? throw new InvalidOperationException("Endpoint provider was not bound"); private set;
    } = null!;

    internal void SetEndpointProvider(IEndpointProvider endpointProvider) =>
        EndpointProvider = endpointProvider;
}