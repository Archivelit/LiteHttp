namespace LiteHttp;

public static class ActionResults
{
    public static IActionResult Continue() => InternalActionResults.Continue();
    public static IActionResult SwitchingProtocols() => InternalActionResults.SwitchingProtocols();
    public static IActionResult Processing() => InternalActionResults.Processing();
    public static IActionResult EarlyHints() => InternalActionResults.EarlyHints();

    public static IActionResult Ok() => InternalActionResults.Ok();
    public static IActionResult Created() => InternalActionResults.Created();
    public static IActionResult Accepted() => InternalActionResults.Accepted();
    public static IActionResult NoContent() => InternalActionResults.NoContent();
    public static IActionResult ResetContent() => InternalActionResults.ResetContent();
    public static IActionResult PartialContent() => InternalActionResults.PartialContent();
    public static IActionResult MultiStatus() => InternalActionResults.MultiStatus();
    public static IActionResult AlreadyReported() => InternalActionResults.AlreadyReported();
    public static IActionResult ImUsed() => InternalActionResults.ImUsed();

    public static IActionResult MultipleChoices() => InternalActionResults.MultipleChoices();
    public static IActionResult MovedPermanently() => InternalActionResults.MovedPermanently();
    public static IActionResult Found() => InternalActionResults.Found();
    public static IActionResult SeeOther() => InternalActionResults.SeeOther();
    public static IActionResult NotModified() => InternalActionResults.NotModified();
    public static IActionResult UseProxy() => InternalActionResults.UseProxy();
    public static IActionResult SwitchProxy() => InternalActionResults.SwitchProxy();
    public static IActionResult TemporaryRedirect() => InternalActionResults.TemporaryRedirect();
    public static IActionResult PermanentRedirect() => InternalActionResults.PermanentRedirect();

    public static IActionResult BadRequest() => InternalActionResults.BadRequest();
    public static IActionResult Unauthorized() => InternalActionResults.Unauthorized();
    public static IActionResult PaymentRequired() => InternalActionResults.PaymentRequired();
    public static IActionResult Forbidden() => InternalActionResults.Forbidden();
    public static IActionResult NotFound() => InternalActionResults.NotFound();
    public static IActionResult MethodNotAllowed() => InternalActionResults.MethodNotAllowed();
    public static IActionResult NotAcceptable() => InternalActionResults.NotAcceptable();
    public static IActionResult ProxyAuthenticationRequired() => InternalActionResults.ProxyAuthenticationRequired();
    public static IActionResult RequestTimeout() => InternalActionResults.RequestTimeout();
    public static IActionResult Conflict() => InternalActionResults.Conflict();
    public static IActionResult Gone() => InternalActionResults.Gone();
    public static IActionResult LengthRequired() => InternalActionResults.LengthRequired();
    public static IActionResult PreconditionFailed() => InternalActionResults.PreconditionFailed();
    public static IActionResult ContentTooLarge() => InternalActionResults.ContentTooLarge();
    public static IActionResult UriTooLong() => InternalActionResults.UriTooLong();
    public static IActionResult UnsupportedMediaType() => InternalActionResults.UnsupportedMediaType();
    public static IActionResult RangeNotSatisfiable() => InternalActionResults.RangeNotSatisfiable();
    public static IActionResult ExpectationFailed() => InternalActionResults.ExpectationFailed();
    public static IActionResult ImATeapot() => InternalActionResults.ImATeapot();
    public static IActionResult MisdirectedRequest() => InternalActionResults.MisdirectedRequest();
    public static IActionResult UnprocessableEntity() => InternalActionResults.UnprocessableEntity();
    public static IActionResult Locked() => InternalActionResults.Locked();
    public static IActionResult FailedDependency() => InternalActionResults.FailedDependency();
    public static IActionResult TooEarly() => InternalActionResults.TooEarly();
    public static IActionResult UpgradeRequired() => InternalActionResults.UpgradeRequired();
    public static IActionResult PreconditionRequired() => InternalActionResults.PreconditionRequired();
    public static IActionResult TooManyRequests() => InternalActionResults.TooManyRequests();
    public static IActionResult RequestHeaderFieldsTooLarge() => InternalActionResults.RequestHeaderFieldsTooLarge();
    public static IActionResult UnavailableForLegalReasons() => InternalActionResults.UnavailableForLegalReasons();

    public static IActionResult InternalServerError() => InternalActionResults.InternalServerError();
    public static IActionResult NotImplemented() => InternalActionResults.NotImplemented();
    public static IActionResult BadGateway() => InternalActionResults.BadGateway();
    public static IActionResult ServiceUnavailable() => InternalActionResults.ServiceUnavailable();
    public static IActionResult GatewayTimeout() => InternalActionResults.GatewayTimeout();
    public static IActionResult HttpVersionNotSupported() => InternalActionResults.HttpVersionNotSupported();
    public static IActionResult VariantAlsoNegotiates() => InternalActionResults.VariantAlsoNegotiates();
    public static IActionResult InsufficientStorage() => InternalActionResults.InsufficientStorage();
    public static IActionResult LoopDetected() => InternalActionResults.LoopDetected();
    public static IActionResult NotExtended() => InternalActionResults.NotExtended();
    public static IActionResult NetworkAuthenticationRequired() => InternalActionResults.NetworkAuthenticationRequired();
}