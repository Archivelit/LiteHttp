namespace LiteHttp.Models;

public record struct HttpContext(
    string Method,
    string Path,
    Dictionary<string, string> Headers,
    string? Body);