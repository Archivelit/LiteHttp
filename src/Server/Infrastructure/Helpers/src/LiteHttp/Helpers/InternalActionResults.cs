using LiteHttp.Enums;
using LiteHttp.Models;

namespace LiteHttp.Helpers;

public static class InternalActionResults
{
    private static readonly ActionResult ContinueResult = new(ResponseCode.Continue);
    private static readonly ActionResult SwitchingProtocolsResult = new(ResponseCode.SwitchingProtocols);
    private static readonly ActionResult ProcessingResult = new(ResponseCode.Processing);
    private static readonly ActionResult EarlyHintsResult = new(ResponseCode.EarlyHints);

    private static readonly ActionResult OkResult = new(ResponseCode.Ok);
    private static readonly ActionResult CreatedResult = new(ResponseCode.Created);
    private static readonly ActionResult AcceptedResult = new(ResponseCode.Accepted);
    private static readonly ActionResult NoContentResult = new(ResponseCode.NoContent);
    private static readonly ActionResult ResetContentResult = new(ResponseCode.ResetContent);
    private static readonly ActionResult PartialContentResult = new(ResponseCode.PartialContent);
    private static readonly ActionResult MultiStatusResult = new(ResponseCode.MultiStatus);
    private static readonly ActionResult AlreadyReportedResult = new(ResponseCode.AlreadyReported);
    private static readonly ActionResult ImUsedResult = new(ResponseCode.ImUsed);

    private static readonly ActionResult MultipleChoicesResult = new(ResponseCode.MultipleChoices);
    private static readonly ActionResult MovedPermanentlyResult = new(ResponseCode.MovedPermanently);
    private static readonly ActionResult FoundResult = new(ResponseCode.Found);
    private static readonly ActionResult SeeOtherResult = new(ResponseCode.SeeOther);
    private static readonly ActionResult NotModifiedResult = new(ResponseCode.NotModified);
    private static readonly ActionResult UseProxyResult = new(ResponseCode.UseProxy);
    private static readonly ActionResult SwitchProxyResult = new(ResponseCode.SwitchProxy);
    private static readonly ActionResult TemporaryRedirectResult = new(ResponseCode.TemporaryRedirect);
    private static readonly ActionResult PermanentRedirectResult = new(ResponseCode.PermanentRedirect);

    private static readonly ActionResult BadRequestResult = new(ResponseCode.BadRequest);
    private static readonly ActionResult UnauthorizedResult = new(ResponseCode.Unauthorized);
    private static readonly ActionResult PaymentRequiredResult = new(ResponseCode.PaymentRequired);
    private static readonly ActionResult ForbiddenResult = new(ResponseCode.Forbidden);
    private static readonly ActionResult NotFoundResult = new(ResponseCode.NotFound);
    private static readonly ActionResult MethodNotAllowedResult = new(ResponseCode.MethodNotAllowed);
    private static readonly ActionResult NotAcceptableResult = new(ResponseCode.NotAcceptable);
    private static readonly ActionResult ProxyAuthenticationRequiredResult = new(ResponseCode.ProxyAuthenticationRequired);
    private static readonly ActionResult RequestTimeoutResult = new(ResponseCode.RequestTimeout);
    private static readonly ActionResult ConflictResult = new(ResponseCode.Conflict);
    private static readonly ActionResult GoneResult = new(ResponseCode.Gone);
    private static readonly ActionResult LengthRequiredResult = new(ResponseCode.LengthRequired);
    private static readonly ActionResult PreconditionFailedResult = new(ResponseCode.PreconditionFailed);
    private static readonly ActionResult ContentTooLargeResult = new(ResponseCode.ContentTooLarge);
    private static readonly ActionResult UriTooLongResult = new(ResponseCode.UriTooLong);
    private static readonly ActionResult UnsupportedMediaTypeResult = new(ResponseCode.UnsupportedMediaType);
    private static readonly ActionResult RangeNotSatisfiableResult = new(ResponseCode.RangeNotSatisfiable);
    private static readonly ActionResult ExpectationFailedResult = new(ResponseCode.ExpectationFailed);
    private static readonly ActionResult ImATeapotResult = new(ResponseCode.ImATeapot);
    private static readonly ActionResult MisdirectedRequestResult = new(ResponseCode.MisdirectedRequest);
    private static readonly ActionResult UnprocessableEntityResult = new(ResponseCode.UnprocessableEntity);
    private static readonly ActionResult LockedResult = new(ResponseCode.Locked);
    private static readonly ActionResult FailedDependencyResult = new(ResponseCode.FailedDependency);
    private static readonly ActionResult TooEarlyResult = new(ResponseCode.TooEarly);
    private static readonly ActionResult UpgradeRequiredResult = new(ResponseCode.UpgradeRequired);
    private static readonly ActionResult PreconditionRequiredResult = new(ResponseCode.PreconditionRequired);
    private static readonly ActionResult TooManyRequestsResult = new(ResponseCode.TooManyRequests);
    private static readonly ActionResult RequestHeaderFieldsTooLargeResult = new(ResponseCode.RequestHeaderFieldsTooLarge);
    private static readonly ActionResult UnavailableForLegalReasonsResult = new(ResponseCode.UnavailableForLegalReasons);

    private static readonly ActionResult InternalServerErrorResult = new(ResponseCode.InternalServerError);
    private static readonly ActionResult NotImplementedResult = new(ResponseCode.NotImplemented);
    private static readonly ActionResult BadGatewayResult = new(ResponseCode.BadGateway);
    private static readonly ActionResult ServiceUnavailableResult = new(ResponseCode.ServiceUnavailable);
    private static readonly ActionResult GatewayTimeoutResult = new(ResponseCode.GatewayTimeout);
    private static readonly ActionResult HttpVersionNotSupportedResult = new(ResponseCode.HttpVersionNotSupported);
    private static readonly ActionResult VariantAlsoNegotiatesResult = new(ResponseCode.VariantAlsoNegotiates);
    private static readonly ActionResult InsufficientStorageResult = new(ResponseCode.InsufficientStorage);
    private static readonly ActionResult LoopDetectedResult = new(ResponseCode.LoopDetected);
    private static readonly ActionResult NotExtendedResult = new(ResponseCode.NotExtended);
    private static readonly ActionResult NetworkAuthenticationRequiredResult = new(ResponseCode.NetworkAuthenticationRequired);

    public static IActionResult Continue() => ContinueResult;
    public static IActionResult SwitchingProtocols() => SwitchingProtocolsResult;
    public static IActionResult Processing() => ProcessingResult;
    public static IActionResult EarlyHints() => EarlyHintsResult;

    public static IActionResult Ok() => OkResult;
    public static IActionResult Created() => CreatedResult;
    public static IActionResult Accepted() => AcceptedResult;
    public static IActionResult NoContent() => NoContentResult;
    public static IActionResult ResetContent() => ResetContentResult;
    public static IActionResult PartialContent() => PartialContentResult;
    public static IActionResult MultiStatus() => MultiStatusResult;
    public static IActionResult AlreadyReported() => AlreadyReportedResult;
    public static IActionResult ImUsed() => ImUsedResult;

    public static IActionResult MultipleChoices() => MultipleChoicesResult;
    public static IActionResult MovedPermanently() => MovedPermanentlyResult;
    public static IActionResult Found() => FoundResult;
    public static IActionResult SeeOther() => SeeOtherResult;
    public static IActionResult NotModified() => NotModifiedResult;
    public static IActionResult UseProxy() => UseProxyResult;
    public static IActionResult SwitchProxy() => SwitchProxyResult;
    public static IActionResult TemporaryRedirect() => TemporaryRedirectResult;
    public static IActionResult PermanentRedirect() => PermanentRedirectResult;

    public static IActionResult BadRequest() => BadRequestResult;
    public static IActionResult Unauthorized() => UnauthorizedResult;
    public static IActionResult PaymentRequired() => PaymentRequiredResult;
    public static IActionResult Forbidden() => ForbiddenResult;
    public static IActionResult NotFound() => NotFoundResult;
    public static IActionResult MethodNotAllowed() => MethodNotAllowedResult;
    public static IActionResult NotAcceptable() => NotAcceptableResult;
    public static IActionResult ProxyAuthenticationRequired() => ProxyAuthenticationRequiredResult;
    public static IActionResult RequestTimeout() => RequestTimeoutResult;
    public static IActionResult Conflict() => ConflictResult;
    public static IActionResult Gone() => GoneResult;
    public static IActionResult LengthRequired() => LengthRequiredResult;
    public static IActionResult PreconditionFailed() => PreconditionFailedResult;
    public static IActionResult ContentTooLarge() => ContentTooLargeResult;
    public static IActionResult UriTooLong() => UriTooLongResult;
    public static IActionResult UnsupportedMediaType() => UnsupportedMediaTypeResult;
    public static IActionResult RangeNotSatisfiable() => RangeNotSatisfiableResult;
    public static IActionResult ExpectationFailed() => ExpectationFailedResult;
    public static IActionResult ImATeapot() => ImATeapotResult;
    public static IActionResult MisdirectedRequest() => MisdirectedRequestResult;
    public static IActionResult UnprocessableEntity() => UnprocessableEntityResult;
    public static IActionResult Locked() => LockedResult;
    public static IActionResult FailedDependency() => FailedDependencyResult;
    public static IActionResult TooEarly() => TooEarlyResult;
    public static IActionResult UpgradeRequired() => UpgradeRequiredResult;
    public static IActionResult PreconditionRequired() => PreconditionRequiredResult;
    public static IActionResult TooManyRequests() => TooManyRequestsResult;
    public static IActionResult RequestHeaderFieldsTooLarge() => RequestHeaderFieldsTooLargeResult;
    public static IActionResult UnavailableForLegalReasons() => UnavailableForLegalReasonsResult;

    public static IActionResult InternalServerError() => InternalServerErrorResult;
    public static IActionResult NotImplemented() => NotImplementedResult;
    public static IActionResult BadGateway() => BadGatewayResult;
    public static IActionResult ServiceUnavailable() => ServiceUnavailableResult;
    public static IActionResult GatewayTimeout() => GatewayTimeoutResult;
    public static IActionResult HttpVersionNotSupported() => HttpVersionNotSupportedResult;
    public static IActionResult VariantAlsoNegotiates() => VariantAlsoNegotiatesResult;
    public static IActionResult InsufficientStorage() => InsufficientStorageResult;
    public static IActionResult LoopDetected() => LoopDetectedResult;
    public static IActionResult NotExtended() => NotExtendedResult;
    public static IActionResult NetworkAuthenticationRequired() => NetworkAuthenticationRequiredResult;
}