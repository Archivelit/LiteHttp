namespace LiteHttp.Server;

public class EndpointProvider(
    ConcurrentDictionary<(string, string), Func<IActionResult>> endpoints
    ) : IEndpointProvider
{
    public Func<IActionResult>? GetEndpoint(string path, string method) =>
        endpoints.GetValueOrDefault((path, method));

    public void AddEndpoint(string path, string method, Func<IActionResult> action) =>
        endpoints.TryAdd((path, method), action);
}