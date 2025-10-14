namespace LiteHttp.RequestProcessors;

public class Router : IRouter
{
    private Dictionary<(string, string), Func<Task<IActionResult>>> MethodMap = new();

    public Func<Task<IActionResult>>? GetAction(string path, string method) => 
        MethodMap.TryGetValue((path, method), out var value) 
        ? value 
        : null;

    public void RegisterAction(string path, string requestMethod, Func<Task<IActionResult>> method) =>
        MethodMap.Add((path, requestMethod), method);
}