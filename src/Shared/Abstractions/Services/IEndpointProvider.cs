namespace LiteHttp.Abstractions;

public interface IEndpointProvider
{
    Func<IActionResult>? GetEndpoint(string path, string method);
    void AddEndpoint(string path, string method, Func<IActionResult> action);
}