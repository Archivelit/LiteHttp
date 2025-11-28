namespace LiteHttp.Abstractions.Factories;

public interface IActionResultFactory
{
    public IActionResult Ok();
    public IActionResult Created();
    public IActionResult Accepted();
    public IActionResult NoContent();
    public IActionResult MultipleChoices();
    public IActionResult NotModified();
    public IActionResult BadRequest();
    public IActionResult Unauthorized();
    public IActionResult Forbidden();
    public IActionResult NotFound();
    public IActionResult MethodNotAllowed();
    public IActionResult RequestTimeout();
    public IActionResult Conflict();
    public IActionResult ContentTooLarge();
    public IActionResult TooManyRequests();
    public IActionResult InternalServerError();
    public IActionResult NotImplemented();
    public IActionResult ServiceUnavailable();
}