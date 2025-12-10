namespace LiteHttp.RequestProcessors;

internal sealed class Router
{
    private IEndpointContext? _endpointContext;

    public Func<IActionResult>? GetAction(in HttpContext context) =>
        _endpointContext?.EndpointProvider.GetEndpoint(context.Route, context.Method);

    public void SetContext(IEndpointContext endpointContext) =>
        _endpointContext = endpointContext;
}