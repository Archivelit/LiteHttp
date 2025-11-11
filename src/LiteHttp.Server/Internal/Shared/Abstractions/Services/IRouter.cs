namespace LiteHttp.Abstractions;

public interface IRouter
{
    Func<IActionResult>? GetAction(in HttpContext context);
    void SetProvider(IEndpointProvider endpointProvider);
}