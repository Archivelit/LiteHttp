namespace LiteHttp.RequestProcessors;

public class Router : IRouter
{
    private IEndpointProvider? _endpointProvider;
    
    public Func<IActionResult>? GetAction(HttpContext context) => 
        _endpointProvider?.GetEndpoint(context.Path, context.Method);

    public void SetProvider(IEndpointProvider endpointProvider) =>
        _endpointProvider = endpointProvider;
}