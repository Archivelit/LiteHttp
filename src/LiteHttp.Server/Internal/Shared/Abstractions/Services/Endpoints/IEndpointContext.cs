namespace LiteHttp.Abstractions;

public interface IEndpointContext
{
    IEndpointProvider EndpointProvider { get; }
}
