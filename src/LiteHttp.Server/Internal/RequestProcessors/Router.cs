namespace LiteHttp.RequestProcessors;

internal sealed class Router : IRouter
{
    private IEndpointProvider? _endpointProvider;
    
    public Func<IActionResult>? GetAction(in HttpContext context) => 
        _endpointProvider?.GetEndpoint(context.Path, context.Method);

    public void SetProvider(IEndpointProvider endpointProvider) =>
        _endpointProvider = endpointProvider;
}