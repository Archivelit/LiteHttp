using LiteHttp.Enums;

namespace UnitTests.LiteHttp.RequestProcessors;

#nullable disable
public class ResponseBuilderTests
{
    private ResponseBuilder _responseBuilder = new();
    private ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;
    private ActionResultFactory _factory = ActionResultFactory.Instance;
    
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
        var actionResult = new ActionResult<string>(ResponseCode.Ok,"Hello, World!");
        
        var expected = "HTTP/1.1 200 OK\r\nHost: " +
                       $"{AddressConstants.IPV4_LOOPBACK.ToString()}" +
                       $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n" +
                       "Content-Type: text/plain\r\n" +
                       "Content-Length: 13\r\n\r\n" +
                       "Hello, World!";

        // Act
        var result = Encoding.UTF8.GetString(_responseBuilder.Build(actionResult).Span);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}