namespace LiteHttp.Abstractions;

public interface IRouteResolver
{
    Func<Task<IActionResult>> ResolveAction(string path, string method);
}