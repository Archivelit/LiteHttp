namespace LiteHttp.Abstractions;

public interface IRouter
{
    public Func<IActionResult>? GetAction(in HttpContext context);
    public void SetContext(IEndpointContext endpointContext);
}