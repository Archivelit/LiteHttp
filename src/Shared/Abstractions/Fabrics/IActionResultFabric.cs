namespace LiteHttp.Abstractions.Fabrics;

public interface IActionResultFabric
{
    IActionResult Ok();
    IActionResult BadRequest();
    IActionResult NotFound();
    IActionResult InternalServerError();
}