namespace Shared.Abstractions.Services;

public interface IResponseGenerator
{
    void Generate(IActionResult actionResult);
}