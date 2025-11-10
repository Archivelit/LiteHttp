using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using Xunit.v3;

namespace UnitTests.LiteHttp.RequestProcessors;

public class ParserTests
{
    private readonly Parser _parser = Parser.Instance;
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;
    
    [Fact]
    public void Parse_ValidRequest_WithoutBody()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\nHost: test.com");

        var expectedHeaders = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>
        {
            { Encoding.UTF8.GetBytes("Host"), Encoding.UTF8.GetBytes("test.com") }
        };
        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Get;
        ReadOnlyMemory<byte>? expectedBody = null;

        // Act
        var result = _parser.Parse(request);
        WriteContextData(result);
        
        // Assert
        result.Method.Should().BeEquivalentTo(expectedMethod);
        result.Path.Should().BeEquivalentTo(expectedRoute);
        result.Headers.Should().BeEquivalentTo(expectedHeaders);
        result.Body.Should().BeEquivalentTo(expectedBody);
    }

    [Fact]
    public void Parse_ValidRequest_WithSimpleBody()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("PUT / HTTP/1.1\r\nHost: test.com\r\nContent-Type: text/plain" +
                                             "\r\nContent-Length: 13\r\n\r\nHello, World!");

        var expectedHeaders = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>
        {
            { Encoding.UTF8.GetBytes("Host"), Encoding.UTF8.GetBytes("test.com") },
            { Encoding.UTF8.GetBytes("Content-Type"), Encoding.UTF8.GetBytes("text/plain") },
            { Encoding.UTF8.GetBytes("Content-Length"), Encoding.UTF8.GetBytes("13") }
        };
        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Put;
        ReadOnlyMemory<byte>? expectedBody = Encoding.UTF8.GetBytes("Hello, World!");

        var expected = new HttpContext(expectedMethod, expectedRoute, expectedHeaders, expectedBody);

        // Act
        var result = _parser.Parse(request);
        WriteContextData(result);
        
        // Assert
        /*result.Method.Should().BeEquivalentTo(expectedMethod);
        result.Path.Should().BeEquivalentTo(expectedRoute);
        result.Headers.Should().BeEquivalentTo(expectedHeaders);
        result.Body.Should().BeEquivalentTo(expectedBody);*/

        result.Should().BeEquivalentTo(expected);
    }

    private void WriteContextData(HttpContext context)
    {
        _outputHelper.WriteLine($"The request route gained after parsing: {Encoding.UTF8.GetString(context.Path.Span)}");
        _outputHelper.WriteLine($"The request method gained after parsing: {Encoding.UTF8.GetString(context.Method.Span)}");
        _outputHelper.WriteLine(context.Body.HasValue
            ? $"Request body gained after parsing: {Encoding.UTF8.GetString(context.Body.Value.Span)}"
            : "Request does not have body");
        _outputHelper.WriteLine($"Request headers gained after parsing: ");
        
        foreach (var header in context.Headers)
        {
            _outputHelper.WriteLine($"{Encoding.UTF8.GetString(header.Key.Span)}: {Encoding.UTF8.GetString(header.Value.Span)}");
        }
    }
}