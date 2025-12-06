namespace UnitTests.LiteHttp.RequestProcessors;

#nullable disable
public class ResponseBuilderTests
{
    private readonly ResponseBuilder _responseBuilder = new();
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;
    private readonly ActionResultFactory _factory = ActionResultFactory.Instance;

    [Fact]
    public void Build_ValidRequest()
    {
        // Arrange
        var actionResult = _factory.Ok();
        var expected = "HTTP/1.1 200 OK\r\nHost: " +
                       $"{AddressConstants.IPV4_LOOPBACK.ToString()}" +
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
                       $"{AddressConstants.IPV4_LOOPBACK.ToString()}" +
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

    public static IEnumerable<object[]> PossibleResponses =>
        new List<object[]>
        {
            new object[]
            {
                new ActionResult(ResponseCode.Ok),
                $"HTTP/1.1 200 OK\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.Created),
                $"HTTP/1.1 201 Created\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.Accepted),
                $"HTTP/1.1 202 Accepted\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.NoContent),
                $"HTTP/1.1 204 No Content\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.MultipleChoices),
                $"HTTP/1.1 300 Multiple Choices\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.NotModified),
                $"HTTP/1.1 304 Not Modified\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.BadRequest),
                $"HTTP/1.1 400 Bad Request\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.Unauthorized),
                $"HTTP/1.1 401 Unauthorized\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.Forbidden),
                $"HTTP/1.1 403 Forbidden\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.NotFound),
                $"HTTP/1.1 404 Not Found\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.MethodNotAllowed),
                $"HTTP/1.1 405 Method Not Allowed\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.RequestTimeout),
                $"HTTP/1.1 408 Request Timeout\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.Conflict),
                $"HTTP/1.1 409 Conflict\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.ContentTooLarge),
                $"HTTP/1.1 413 Content Too Large\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.TooManyRequests),
                $"HTTP/1.1 429 Too Many Requests\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.InternalServerError),
                $"HTTP/1.1 500 Internal Server Error\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.NotImplemented),
                $"HTTP/1.1 501 Not Implemented\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            },
            new object[]
            {
                new ActionResult(ResponseCode.ServiceUnavailable),
                $"HTTP/1.1 503 Service Unavailable\r\nHost: {AddressConstants.IPV4_LOOPBACK.ToString()}" +
                $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n"
            }
        };
}