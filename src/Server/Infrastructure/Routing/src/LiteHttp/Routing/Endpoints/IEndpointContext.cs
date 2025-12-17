namespace LiteHttp.Routing;

public interface IEndpointContext
{
    public IEndpointProvider EndpointProvider { get; }
}