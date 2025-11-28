namespace LiteHttp.Server;

public class ActionResultFactory : IActionResultFactory
{
    public static readonly ActionResultFactory Instance = new();

    private static readonly ActionResult _okResult = new(ResponseCode.Ok);
    private static readonly ActionResult _createdResult = new(ResponseCode.Created);
    private static readonly ActionResult _acceptedResult = new(ResponseCode.Accepted);
    private static readonly ActionResult _noContentResult = new(ResponseCode.NoContent);
    private static readonly ActionResult _multipleChoicesResult = new(ResponseCode.MultipleChoices);
    private static readonly ActionResult _notModifiedResult = new(ResponseCode.NotModified);
    private static readonly ActionResult _badRequestResult = new(ResponseCode.BadRequest);
    private static readonly ActionResult _unauthorizedResult = new(ResponseCode.Unauthorized);
    private static readonly ActionResult _forbiddenResult = new(ResponseCode.Forbidden);
    private static readonly ActionResult _notFoundResult = new(ResponseCode.NotFound);
    private static readonly ActionResult _methodNotAllowedResult = new(ResponseCode.MethodNotAllowed);
    private static readonly ActionResult _requestTimeoutResult = new(ResponseCode.RequestTimeout);
    private static readonly ActionResult _conflictResult = new(ResponseCode.Conflict);
    private static readonly ActionResult _contentTooLargeResult = new(ResponseCode.ContentTooLarge);
    private static readonly ActionResult _tooManyRequestsResult = new(ResponseCode.TooManyRequests);
    private static readonly ActionResult _internalServerErrorResult = new(ResponseCode.InternalServerError);
    private static readonly ActionResult _notImplementedResult = new(ResponseCode.NotImplemented);
    private static readonly ActionResult _serviceUnavailableResult = new(ResponseCode.ServiceUnavailable);

    public IActionResult Ok() => _okResult;
    public IActionResult Created() => _createdResult;
    public IActionResult Accepted() => _acceptedResult;
    public IActionResult NoContent() => _noContentResult;
    public IActionResult MultipleChoices() => _multipleChoicesResult;
    public IActionResult NotModified() => _notModifiedResult;
    public IActionResult BadRequest() => _badRequestResult;
    public IActionResult Unauthorized() => _unauthorizedResult;
    public IActionResult Forbidden() => _forbiddenResult;
    public IActionResult NotFound() => _notFoundResult;
    public IActionResult MethodNotAllowed() => _methodNotAllowedResult;
    public IActionResult RequestTimeout() => _requestTimeoutResult;
    public IActionResult Conflict() => _conflictResult;
    public IActionResult ContentTooLarge() => _contentTooLargeResult;
    public IActionResult TooManyRequests() => _tooManyRequestsResult;
    public IActionResult InternalServerError() => _internalServerErrorResult;
    public IActionResult NotImplemented() => _notImplementedResult;
    public IActionResult ServiceUnavailable() => _serviceUnavailableResult;
}