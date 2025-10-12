namespace LiteHttp.Models;

public record struct HttpContext(
    Dictionary<string, string> Headers,
    string Body);