namespace LiteHttp.Abstractions.Factories;

public interface IActionResultFactory
{
    public IActionResult Ok();
    public IActionResult BadRequest();
    public IActionResult NotFound();
    public IActionResult InternalServerError();
}