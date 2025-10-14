using Shared.Abstractions;

namespace LiteHttp.Abstractions;

public interface IRouteResolver
{
    Func<Task<IActionResult>>? GetAction(string path, string method);
    void RegisterAction(string path, string requestMethod, Func<Task<IActionResult>> action);
}