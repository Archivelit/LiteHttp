namespace LiteHttp.Abstractions;

public interface IEndpointContext
{
    public IEndpointProvider EndpointProvider { get; }
}