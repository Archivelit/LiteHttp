namespace LiteHttp.Abstractions;

internal interface IEndpointProviderConfiguration
{
    IEndpointContext EndpointContext { get; }
    void Freeze();
    void AddEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method, Func<IActionResult> action);
}
