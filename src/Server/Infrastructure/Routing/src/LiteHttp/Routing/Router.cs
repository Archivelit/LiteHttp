namespace LiteHttp.Routing;

internal sealed class Router : IRouter
{
    private IEndpointContext? _endpointContext;

    public Func<IActionResult>? GetAction(HttpContext context) =>
        _endpointContext?.EndpointProvider.GetEndpoint(context.Route, context.Method);

    public void SetContext(IEndpointContext endpointContext) =>
        _endpointContext = endpointContext;
}