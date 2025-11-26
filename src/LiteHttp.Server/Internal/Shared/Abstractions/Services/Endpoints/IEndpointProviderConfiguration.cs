namespace LiteHttp.Abstractions;

internal interface IEndpointProviderConfiguration
{
    public IEndpointContext EndpointContext { get; }
    public void Freeze();
    public void AddEndpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method, Func<IActionResult> action);
}