namespace LiteHttp.Abstractions;

public interface IRouter
{
    Func<IActionResult>? GetAction(string path, string method);
    void SetProvider(IEndpointProvider endpointProvider);
}