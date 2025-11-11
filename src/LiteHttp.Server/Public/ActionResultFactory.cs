namespace LiteHttp.Server;

public class ActionResultFactory : IActionResultFactory
{
    public static readonly ActionResultFactory Instance = new();

    private static readonly ActionResult _okResult = new(ResponseCode.Ok);
    private static readonly ActionResult _errorResult = new(ResponseCode.InternalServerError);
    private static readonly ActionResult _notFoundResult = new(ResponseCode.NotFound);
    private static readonly ActionResult _badRequestResult = new(ResponseCode.BadRequest);

    public IActionResult Ok() => _okResult;

    public IActionResult BadRequest() => _badRequestResult;

    public IActionResult NotFound() => _notFoundResult;

    public IActionResult InternalServerError() => _errorResult;
}