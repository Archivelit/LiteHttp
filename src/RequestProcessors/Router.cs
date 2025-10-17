namespace LiteHttp.RequestProcessors;

public class Router : IRouter
{
    private IEndpointProvider _endpointProvider;
    
    public Func<IActionResult>? GetAction(string path, string method) => 
        _endpointProvider.GetEndpoint(path, method);

    public void SetProvider(IEndpointProvider endpointProvider) =>
        _endpointProvider = endpointProvider;
}