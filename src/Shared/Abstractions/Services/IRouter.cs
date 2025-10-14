namespace LiteHttp.Abstractions;

public interface IRouter
{
    Func<Task<IActionResult>>? GetAction(string path, string method);
    void RegisterAction(string path, string requestMethod, Func<Task<IActionResult>> action);
}