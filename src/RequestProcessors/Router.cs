namespace LiteHttp.RequestProcessors;

public class Router : IRouter
{
    private Dictionary<(string, string), Func<IActionResult>> MethodMap = new();

    public Func<IActionResult>? GetAction(string path, string method) => 
        MethodMap.GetValueOrDefault((path, method));

    public void RegisterAction(string path, string requestMethod, Func<IActionResult> method) =>
        MethodMap.Add((path, requestMethod), method);
}