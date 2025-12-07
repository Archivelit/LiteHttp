namespace LiteHttp.Server;

public class ActionResultFactory : IActionResultFactory
{
    public static readonly ActionResultFactory Instance = new();

    private static readonly ActionResult _continueResult = new(ResponseCode.Continue);
    private static readonly ActionResult _switchingProtocolsResult = new(ResponseCode.SwitchingProtocols);
    private static readonly ActionResult _processingResult = new(ResponseCode.Processing);
    private static readonly ActionResult _earlyHintsResult = new(ResponseCode.EarlyHints);

    private static readonly ActionResult _okResult = new(ResponseCode.Ok);
    private static readonly ActionResult _createdResult = new(ResponseCode.Created);
    private static readonly ActionResult _acceptedResult = new(ResponseCode.Accepted);
    private static readonly ActionResult _noContentResult = new(ResponseCode.NoContent);
    private static readonly ActionResult _resetContentResult = new(ResponseCode.ResetContent);
    private static readonly ActionResult _partialContentResult = new(ResponseCode.PartialContent);
    private static readonly ActionResult _multiStatusResult = new(ResponseCode.MultiStatus);
    private static readonly ActionResult _alreadyReportedResult = new(ResponseCode.AlreadyReported);
    private static readonly ActionResult _imUsedResult = new(ResponseCode.ImUsed);

    private static readonly ActionResult _multipleChoicesResult = new(ResponseCode.MultipleChoices);
    private static readonly ActionResult _movedPermanentlyResult = new(ResponseCode.MovedPermanently);
    private static readonly ActionResult _foundResult = new(ResponseCode.Found);
    private static readonly ActionResult _seeOtherResult = new(ResponseCode.SeeOther);
    private static readonly ActionResult _notModifiedResult = new(ResponseCode.NotModified);
    private static readonly ActionResult _useProxyResult = new(ResponseCode.UseProxy);
    private static readonly ActionResult _switchProxyResult = new(ResponseCode.SwitchProxy);
    private static readonly ActionResult _temporaryRedirectResult = new(ResponseCode.TemporaryRedirect);
    private static readonly ActionResult _permanentRedirectResult = new(ResponseCode.PermanentRedirect);

    private static readonly ActionResult _badRequestResult = new(ResponseCode.BadRequest);
    private static readonly ActionResult _unauthorizedResult = new(ResponseCode.Unauthorized);
    private static readonly ActionResult _paymentRequiredResult = new(ResponseCode.PaymentRequired);
    private static readonly ActionResult _forbiddenResult = new(ResponseCode.Forbidden);
    private static readonly ActionResult _notFoundResult = new(ResponseCode.NotFound);
    private static readonly ActionResult _methodNotAllowedResult = new(ResponseCode.MethodNotAllowed);
    private static readonly ActionResult _notAcceptableResult = new(ResponseCode.NotAcceptable);
    private static readonly ActionResult _proxyAuthenticationRequiredResult = new(ResponseCode.ProxyAuthenticationRequired);
    private static readonly ActionResult _requestTimeoutResult = new(ResponseCode.RequestTimeout);
    private static readonly ActionResult _conflictResult = new(ResponseCode.Conflict);
    private static readonly ActionResult _goneResult = new(ResponseCode.Gone);
    private static readonly ActionResult _lengthRequiredResult = new(ResponseCode.LengthRequired);
    private static readonly ActionResult _preconditionFailedResult = new(ResponseCode.PreconditionFailed);
    private static readonly ActionResult _contentTooLargeResult = new(ResponseCode.ContentTooLarge);
    private static readonly ActionResult _uriTooLongResult = new(ResponseCode.UriTooLong);
    private static readonly ActionResult _unsupportedMediaTypeResult = new(ResponseCode.UnsupportedMediaType);
    private static readonly ActionResult _rangeNotSatisfiableResult = new(ResponseCode.RangeNotSatisfiable);
    private static readonly ActionResult _expectationFailedResult = new(ResponseCode.ExpectationFailed);
    private static readonly ActionResult _imATeapotResult = new(ResponseCode.ImATeapot);
    private static readonly ActionResult _misdirectedRequestResult = new(ResponseCode.MisdirectedRequest);
    private static readonly ActionResult _unprocessableEntityResult = new(ResponseCode.UnprocessableEntity);
    private static readonly ActionResult _lockedResult = new(ResponseCode.Locked);
    private static readonly ActionResult _failedDependencyResult = new(ResponseCode.FailedDependency);
    private static readonly ActionResult _tooEarlyResult = new(ResponseCode.TooEarly);
    private static readonly ActionResult _upgradeRequiredResult = new(ResponseCode.UpgradeRequired);
    private static readonly ActionResult _preconditionRequiredResult = new(ResponseCode.PreconditionRequired);
    private static readonly ActionResult _tooManyRequestsResult = new(ResponseCode.TooManyRequests);
    private static readonly ActionResult _requestHeaderFieldsTooLargeResult = new(ResponseCode.RequestHeaderFieldsTooLarge);
    private static readonly ActionResult _unavailableForLegalReasonsResult = new(ResponseCode.UnavailableForLegalReasons);

    private static readonly ActionResult _internalServerErrorResult = new(ResponseCode.InternalServerError);
    private static readonly ActionResult _notImplementedResult = new(ResponseCode.NotImplemented);
    private static readonly ActionResult _badGatewayResult = new(ResponseCode.BadGateway);
    private static readonly ActionResult _serviceUnavailableResult = new(ResponseCode.ServiceUnavailable);
    private static readonly ActionResult _gatewayTimeoutResult = new(ResponseCode.GatewayTimeout);
    private static readonly ActionResult _httpVersionNotSupportedResult = new(ResponseCode.HttpVersionNotSupported);
    private static readonly ActionResult _variantAlsoNegotiatesResult = new(ResponseCode.VariantAlsoNegotiates);
    private static readonly ActionResult _insufficientStorageResult = new(ResponseCode.InsufficientStorage);
    private static readonly ActionResult _loopDetectedResult = new(ResponseCode.LoopDetected);
    private static readonly ActionResult _notExtendedResult = new(ResponseCode.NotExtended);
    private static readonly ActionResult _networkAuthenticationRequiredResult = new(ResponseCode.NetworkAuthenticationRequired);

    public IActionResult Continue() => _continueResult;
    public IActionResult SwitchingProtocols() => _switchingProtocolsResult;
    public IActionResult Processing() => _processingResult;
    public IActionResult EarlyHints() => _earlyHintsResult;

    public IActionResult Ok() => _okResult;
    public IActionResult Created() => _createdResult;
    public IActionResult Accepted() => _acceptedResult;
    public IActionResult NoContent() => _noContentResult;
    public IActionResult ResetContent() => _resetContentResult;
    public IActionResult PartialContent() => _partialContentResult;
    public IActionResult MultiStatus() => _multiStatusResult;
    public IActionResult AlreadyReported() => _alreadyReportedResult;
    public IActionResult ImUsed() => _imUsedResult;

    public IActionResult MultipleChoices() => _multipleChoicesResult;
    public IActionResult MovedPermanently() => _movedPermanentlyResult;
    public IActionResult Found() => _foundResult;
    public IActionResult SeeOther() => _seeOtherResult;
    public IActionResult NotModified() => _notModifiedResult;
    public IActionResult UseProxy() => _useProxyResult;
    public IActionResult SwitchProxy() => _switchProxyResult;
    public IActionResult TemporaryRedirect() => _temporaryRedirectResult;
    public IActionResult PermanentRedirect() => _permanentRedirectResult;

    public IActionResult BadRequest() => _badRequestResult;
    public IActionResult Unauthorized() => _unauthorizedResult;
    public IActionResult PaymentRequired() => _paymentRequiredResult;
    public IActionResult Forbidden() => _forbiddenResult;
    public IActionResult NotFound() => _notFoundResult;
    public IActionResult MethodNotAllowed() => _methodNotAllowedResult;
    public IActionResult NotAcceptable() => _notAcceptableResult;
    public IActionResult ProxyAuthenticationRequired() => _proxyAuthenticationRequiredResult;
    public IActionResult RequestTimeout() => _requestTimeoutResult;
    public IActionResult Conflict() => _conflictResult;
    public IActionResult Gone() => _goneResult;
    public IActionResult LengthRequired() => _lengthRequiredResult;
    public IActionResult PreconditionFailed() => _preconditionFailedResult;
    public IActionResult ContentTooLarge() => _contentTooLargeResult;
    public IActionResult UriTooLong() => _uriTooLongResult;
    public IActionResult UnsupportedMediaType() => _unsupportedMediaTypeResult;
    public IActionResult RangeNotSatisfiable() => _rangeNotSatisfiableResult;
    public IActionResult ExpectationFailed() => _expectationFailedResult;
    public IActionResult ImATeapot() => _imATeapotResult;
    public IActionResult MisdirectedRequest() => _misdirectedRequestResult;
    public IActionResult UnprocessableEntity() => _unprocessableEntityResult;
    public IActionResult Locked() => _lockedResult;
    public IActionResult FailedDependency() => _failedDependencyResult;
    public IActionResult TooEarly() => _tooEarlyResult;
    public IActionResult UpgradeRequired() => _upgradeRequiredResult;
    public IActionResult PreconditionRequired() => _preconditionRequiredResult;
    public IActionResult TooManyRequests() => _tooManyRequestsResult;
    public IActionResult RequestHeaderFieldsTooLarge() => _requestHeaderFieldsTooLargeResult;
    public IActionResult UnavailableForLegalReasons() => _unavailableForLegalReasonsResult;

    public IActionResult InternalServerError() => _internalServerErrorResult;
    public IActionResult NotImplemented() => _notImplementedResult;
    public IActionResult BadGateway() => _badGatewayResult;
    public IActionResult ServiceUnavailable() => _serviceUnavailableResult;
    public IActionResult GatewayTimeout() => _gatewayTimeoutResult;
    public IActionResult HttpVersionNotSupported() => _httpVersionNotSupportedResult;
    public IActionResult VariantAlsoNegotiates() => _variantAlsoNegotiatesResult;
    public IActionResult InsufficientStorage() => _insufficientStorageResult;
    public IActionResult LoopDetected() => _loopDetectedResult;
    public IActionResult NotExtended() => _notExtendedResult;
    public IActionResult NetworkAuthenticationRequired() => _networkAuthenticationRequiredResult;
}