namespace LiteHttp.Abstractions;

public interface IRouter
{
    Func<IActionResult>? GetAction(string path, string method);
    void RegisterAction(string path, string requestMethod, Func<IActionResult> action);
}