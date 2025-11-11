namespace LiteHttp.Abstractions.Factories;

public interface IActionResultFactory
{
    IActionResult Ok();
    IActionResult BadRequest();
    IActionResult NotFound();
    IActionResult InternalServerError();
}