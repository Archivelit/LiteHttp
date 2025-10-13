namespace LiteHttp.RequestProcessors;

public class RouteResolver : IRouteResolver
{
    private Dictionary<int, Func<Task<IActionResult>>> MethodMap;

    public Func<Task<IActionResult>> ResolveAction(string path, string method) =>
        MethodMap[GetHashCodeOf(path, method)];
    
    public void AddMethod(string path, string requestMethod, Func<Task<IActionResult>> method) =>
        MethodMap.Add(GetHashCodeOf(path, requestMethod), method);

    private int GetHashCodeOf(string path, string method) =>
        path.GetHashCode() + method.GetHashCode();
}