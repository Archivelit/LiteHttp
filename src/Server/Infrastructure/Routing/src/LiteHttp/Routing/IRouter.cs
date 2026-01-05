namespace LiteHttp.Routing;

public interface IRouter
{
    public Func<IActionResult>? GetAction(HttpContext context);
}