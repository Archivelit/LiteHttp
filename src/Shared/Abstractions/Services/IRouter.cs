namespace LiteHttp.Abstractions;

public interface IRouter
{
    Func<IActionResult>? GetAction(HttpContext context);
    void SetProvider(IEndpointProvider endpointProvider);
}