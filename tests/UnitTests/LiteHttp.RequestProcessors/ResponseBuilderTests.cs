namespace UnitTests.LiteHttp.RequestProcessors;

#nullable disable
public class ResponseBuilderTests
{
    private readonly ResponseBuilder _responseBuilder = new();

    [Fact]
    public void Build_ValidRequest()
    {
        // Arrange
        var actionResult = ActionResultFactory.Ok();
        var expected = "HTTP/1.1 200 OK\r\nHost: " +
                       $"{AddressConstants.IPV4_LOOPBACK}" +
                       $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n";

        // Act
        var result = Encoding.UTF8.GetString(_responseBuilder.Build(actionResult).Span);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Build_ValidRequest_WithBody()
    {
        // Arrange
        var actionResult = new ActionResult<string>(ResponseCode.Ok, "Hello, World!");

        var expected = "HTTP/1.1 200 OK\r\nHost: " +
                       $"{AddressConstants.IPV4_LOOPBACK}" +
                       $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n" +
                       "Content-Type: text/plain\r\n" +
                       "Content-Length: 13\r\n\r\n" +
                       "Hello, World!";

        // Act
        var result = Encoding.UTF8.GetString(_responseBuilder.Build(actionResult, Encoding.UTF8.GetBytes(actionResult.Result)).Span);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [MemberData(nameof(PossibleResponses))]
    public void Build_ValidActionResults_ReturnsExpectedResponse(ActionResult actionResult, string expectedResponse)
    {
        // Act
        var actualResponse = Encoding.UTF8.GetString(_responseBuilder.Build(actionResult).Span);

        // Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }

    public static TheoryData<ActionResult, string> PossibleResponses =>
    new TheoryData<ActionResult, string>
    {
        {
            new ActionResult(ResponseCode.Continue),
            $"HTTP/1.1 100 Continue\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.SwitchingProtocols),
            $"HTTP/1.1 101 Switching Protocols\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Processing),
            $"HTTP/1.1 102 Processing\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.EarlyHints),
            $"HTTP/1.1 103 Early Hints\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Ok),
            $"HTTP/1.1 200 OK\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Created),
            $"HTTP/1.1 201 Created\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Accepted),
            $"HTTP/1.1 202 Accepted\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NoContent),
            $"HTTP/1.1 204 No Content\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.MultiStatus),
            $"HTTP/1.1 207 Multi Status\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.AlreadyReported),
            $"HTTP/1.1 208 Already Reported\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ImUsed),
            $"HTTP/1.1 226 Im Used\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.MultipleChoices),
            $"HTTP/1.1 300 Multiple Choices\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.MovedPermanently),
            $"HTTP/1.1 301 Moved Permanently\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Found),
            $"HTTP/1.1 302 Found\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.SeeOther),
            $"HTTP/1.1 303 See Other\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NotModified),
            $"HTTP/1.1 304 Not Modified\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UseProxy),
            $"HTTP/1.1 305 Use Proxy\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.SwitchProxy),
            $"HTTP/1.1 306 Switch Proxy\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.TemporaryRedirect),
            $"HTTP/1.1 307 Temporary Redirect\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.PermanentRedirect),
            $"HTTP/1.1 308 Permanent Redirect\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.BadRequest),
            $"HTTP/1.1 400 Bad Request\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Unauthorized),
            $"HTTP/1.1 401 Unauthorized\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.PaymentRequired),
            $"HTTP/1.1 402 Payment Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Forbidden),
            $"HTTP/1.1 403 Forbidden\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NotFound),
            $"HTTP/1.1 404 Not Found\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.MethodNotAllowed),
            $"HTTP/1.1 405 Method Not Allowed\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NotAcceptable),
            $"HTTP/1.1 406 Not Acceptable\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ProxyAuthenticationRequired),
            $"HTTP/1.1 407 Proxy Authentication Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.RequestTimeout),
            $"HTTP/1.1 408 Request Timeout\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Conflict),
            $"HTTP/1.1 409 Conflict\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Gone),
            $"HTTP/1.1 410 Gone\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.LengthRequired),
            $"HTTP/1.1 411 Length Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.PreconditionFailed),
            $"HTTP/1.1 412 Precondition Failed\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ContentTooLarge),
            $"HTTP/1.1 413 Content Too Large\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UriTooLong),
            $"HTTP/1.1 414 URI Too Long\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UnsupportedMediaType),
            $"HTTP/1.1 415 Unsupported Media Type\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.RangeNotSatisfiable),
            $"HTTP/1.1 416 Range Not Satisfiable\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ExpectationFailed),
            $"HTTP/1.1 417 Expectation Failed\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ImATeapot),
            $"HTTP/1.1 418 I'm a teapot\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.MisdirectedRequest),
            $"HTTP/1.1 421 Misdirected Request\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UnprocessableEntity),
            $"HTTP/1.1 422 Unprocessable Entity\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.Locked),
            $"HTTP/1.1 423 Locked\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.FailedDependency),
            $"HTTP/1.1 424 Failed Dependency\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.TooEarly),
            $"HTTP/1.1 425 Too Early\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UpgradeRequired),
            $"HTTP/1.1 426 Upgrade Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.PreconditionRequired),
            $"HTTP/1.1 428 Precondition Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.TooManyRequests),
            $"HTTP/1.1 429 Too Many Requests\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.RequestHeaderFieldsTooLarge),
            $"HTTP/1.1 431 Request Header Fields Too Large\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.UnavailableForLegalReasons),
            $"HTTP/1.1 451 Unavailable For Legal Reasons\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.InternalServerError),
            $"HTTP/1.1 500 Internal Server Error\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NotImplemented),
            $"HTTP/1.1 501 Not Implemented\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.BadGateway),
            $"HTTP/1.1 502 Bad Gateway\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.ServiceUnavailable),
            $"HTTP/1.1 503 Service Unavailable\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.GatewayTimeout),
            $"HTTP/1.1 504 Gateway Timeout\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.HttpVersionNotSupported),
            $"HTTP/1.1 505 HTTP Version Not Supported\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.VariantAlsoNegotiates),
            $"HTTP/1.1 506 Variant Also Negotiates\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.InsufficientStorage),
            $"HTTP/1.1 507 Insufficient Storage\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.LoopDetected),
            $"HTTP/1.1 508 Loop Detected\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NotExtended),
            $"HTTP/1.1 510 Not Extended\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        },
        {
            new ActionResult(ResponseCode.NetworkAuthenticationRequired),
            $"HTTP/1.1 511 Network Authentication Required\r\nHost: {AddressConstants.IPV4_LOOPBACK}" +
            $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
        }
    };
}