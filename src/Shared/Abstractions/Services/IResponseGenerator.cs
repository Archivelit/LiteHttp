namespace LiteHttp.Abstractions;

public interface IResponseGenerator
{
    string Generate(IActionResult actionResult, string? responseBody = null);
}