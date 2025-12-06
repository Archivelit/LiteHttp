namespace LiteHttp.Abstractions.Factories;

public interface IActionResultFactory
{
    // 1xx Informational
    public IActionResult Continue();
    public IActionResult SwitchingProtocols();
    public IActionResult Processing();
    public IActionResult EarlyHints();

    // 2xx Success
    public IActionResult Ok();
    public IActionResult Created();
    public IActionResult Accepted();
    public IActionResult NoContent();
    public IActionResult ResetContent();
    public IActionResult PartialContent();
    public IActionResult MultiStatus();
    public IActionResult AlreadyReported();
    public IActionResult ImUsed();

    // 3xx Redirection
    public IActionResult MultipleChoices();
    public IActionResult MovedPermanently();
    public IActionResult Found();
    public IActionResult SeeOther();
    public IActionResult NotModified();
    public IActionResult UseProxy();
    public IActionResult SwitchProxy();
    public IActionResult TemporaryRedirect();
    public IActionResult PermanentRedirect();

    // 4xx Client Errors
    public IActionResult BadRequest();
    public IActionResult Unauthorized();
    public IActionResult PaymentRequired();
    public IActionResult Forbidden();
    public IActionResult NotFound();
    public IActionResult MethodNotAllowed();
    public IActionResult NotAcceptable();
    public IActionResult ProxyAuthenticationRequired();
    public IActionResult RequestTimeout();
    public IActionResult Conflict();
    public IActionResult Gone();
    public IActionResult LengthRequired();
    public IActionResult PreconditionFailed();
    public IActionResult ContentTooLarge();
    public IActionResult UriTooLong();
    public IActionResult UnsupportedMediaType();
    public IActionResult RangeNotSatisfiable();
    public IActionResult ExpectationFailed();
    public IActionResult ImATeapot();
    public IActionResult MisdirectedRequest();
    public IActionResult UnprocessableEntity();
    public IActionResult Locked();
    public IActionResult FailedDependency();
    public IActionResult TooEarly();
    public IActionResult UpgradeRequired();
    public IActionResult PreconditionRequired();
    public IActionResult TooManyRequests();
    public IActionResult RequestHeaderFieldsTooLarge();
    public IActionResult UnavailableForLegalReasons();

    // 5xx Server Errors
    public IActionResult InternalServerError();
    public IActionResult NotImplemented();
    public IActionResult BadGateway();
    public IActionResult ServiceUnavailable();
    public IActionResult GatewayTimeout();
    public IActionResult HttpVersionNotSupported();
    public IActionResult VariantAlsoNegotiates();
    public IActionResult InsufficientStorage();
    public IActionResult LoopDetected();
    public IActionResult NotExtended();
    public IActionResult NetworkAuthenticationRequired();
}
