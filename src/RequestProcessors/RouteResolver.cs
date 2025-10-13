namespace LiteHttp.RequestProcessors;

public class RouteResolver : IRouteResolver
{
    private Dictionary<int, Func<Task<IActionResult>>> MethodMap = new();

    public Func<Task<IActionResult>>? GetAction(string path, string method)
    {
        var operationSuccessful = MethodMap.TryGetValue(GetHashCodeOf(path, method), out var value);

        if (!operationSuccessful)
            return null;
        
        return value;
    }

    public void RegisterAction(string path, string requestMethod, Func<Task<IActionResult>> method) =>
        MethodMap.Add(GetHashCodeOf(path, requestMethod), method);

    private int GetHashCodeOf(string path, string method) =>
        path.GetHashCode() + method.GetHashCode();
}