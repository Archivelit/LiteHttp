namespace LiteHttp.RequestProcessors;

public class Router : IRouter
{
    private Dictionary<(string, string), Func<IActionResult>> _endpoints = new();

    public Func<IActionResult>? GetAction(string path, string method) => 
        _endpoints.GetValueOrDefault((path, method));

    public void RegisterAction(string path, string requestMethod, Func<IActionResult> method) =>
        _endpoints.Add((path, requestMethod), method);

    public void SetMap(Dictionary<(string, string), Func<IActionResult>> map) =>
        _endpoints = map;
}