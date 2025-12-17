namespace LiteHttp;

public static class ActionResultFactory
{
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

    public static IActionResult Continue() => _continueResult;
    public static IActionResult SwitchingProtocols() => _switchingProtocolsResult;
    public static IActionResult Processing() => _processingResult;
    public static IActionResult EarlyHints() => _earlyHintsResult;

    public static IActionResult Ok() => _okResult;
    public static IActionResult Created() => _createdResult;
    public static IActionResult Accepted() => _acceptedResult;
    public static IActionResult NoContent() => _noContentResult;
    public static IActionResult ResetContent() => _resetContentResult;
    public static IActionResult PartialContent() => _partialContentResult;
    public static IActionResult MultiStatus() => _multiStatusResult;
    public static IActionResult AlreadyReported() => _alreadyReportedResult;
    public static IActionResult ImUsed() => _imUsedResult;

    public static IActionResult MultipleChoices() => _multipleChoicesResult;
    public static IActionResult MovedPermanently() => _movedPermanentlyResult;
    public static IActionResult Found() => _foundResult;
    public static IActionResult SeeOther() => _seeOtherResult;
    public static IActionResult NotModified() => _notModifiedResult;
    public static IActionResult UseProxy() => _useProxyResult;
    public static IActionResult SwitchProxy() => _switchProxyResult;
    public static IActionResult TemporaryRedirect() => _temporaryRedirectResult;
    public static IActionResult PermanentRedirect() => _permanentRedirectResult;

    public static IActionResult BadRequest() => _badRequestResult;
    public static IActionResult Unauthorized() => _unauthorizedResult;
    public static IActionResult PaymentRequired() => _paymentRequiredResult;
    public static IActionResult Forbidden() => _forbiddenResult;
    public static IActionResult NotFound() => _notFoundResult;
    public static IActionResult MethodNotAllowed() => _methodNotAllowedResult;
    public static IActionResult NotAcceptable() => _notAcceptableResult;
    public static IActionResult ProxyAuthenticationRequired() => _proxyAuthenticationRequiredResult;
    public static IActionResult RequestTimeout() => _requestTimeoutResult;
    public static IActionResult Conflict() => _conflictResult;
    public static IActionResult Gone() => _goneResult;
    public static IActionResult LengthRequired() => _lengthRequiredResult;
    public static IActionResult PreconditionFailed() => _preconditionFailedResult;
    public static IActionResult ContentTooLarge() => _contentTooLargeResult;
    public static IActionResult UriTooLong() => _uriTooLongResult;
    public static IActionResult UnsupportedMediaType() => _unsupportedMediaTypeResult;
    public static IActionResult RangeNotSatisfiable() => _rangeNotSatisfiableResult;
    public static IActionResult ExpectationFailed() => _expectationFailedResult;
    public static IActionResult ImATeapot() => _imATeapotResult;
    public static IActionResult MisdirectedRequest() => _misdirectedRequestResult;
    public static IActionResult UnprocessableEntity() => _unprocessableEntityResult;
    public static IActionResult Locked() => _lockedResult;
    public static IActionResult FailedDependency() => _failedDependencyResult;
    public static IActionResult TooEarly() => _tooEarlyResult;
    public static IActionResult UpgradeRequired() => _upgradeRequiredResult;
    public static IActionResult PreconditionRequired() => _preconditionRequiredResult;
    public static IActionResult TooManyRequests() => _tooManyRequestsResult;
    public static IActionResult RequestHeaderFieldsTooLarge() => _requestHeaderFieldsTooLargeResult;
    public static IActionResult UnavailableForLegalReasons() => _unavailableForLegalReasonsResult;

    public static IActionResult InternalServerError() => _internalServerErrorResult;
    public static IActionResult NotImplemented() => _notImplementedResult;
    public static IActionResult BadGateway() => _badGatewayResult;
    public static IActionResult ServiceUnavailable() => _serviceUnavailableResult;
    public static IActionResult GatewayTimeout() => _gatewayTimeoutResult;
    public static IActionResult HttpVersionNotSupported() => _httpVersionNotSupportedResult;
    public static IActionResult VariantAlsoNegotiates() => _variantAlsoNegotiatesResult;
    public static IActionResult InsufficientStorage() => _insufficientStorageResult;
    public static IActionResult LoopDetected() => _loopDetectedResult;
    public static IActionResult NotExtended() => _notExtendedResult;
    public static IActionResult NetworkAuthenticationRequired() => _networkAuthenticationRequiredResult;
}