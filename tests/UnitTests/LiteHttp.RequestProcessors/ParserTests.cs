namespace UnitTests.LiteHttp.RequestProcessors;

#nullable disable
public class ParserTests
{
    private readonly Parser _parser = Parser.Instance;
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;
    
    [Fact]
    public void Parse_ValidRequest_WithoutBody_WithCRLFOnEnd()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\nHost: test.com\r\n");

        var expectedHeaders = new Dictionary<string, string>
        {
            { "Host", "test.com" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Get;
        ReadOnlyMemory<byte>? expectedBody = null;

        // Act
        var result = _parser.Parse(request).Value;
        WriteContextData(result);
        
        // Assert
        result.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Body.Should().BeNull();
        result.Body.Should().BeEquivalentTo(expectedBody);
    }

    [Fact]
    public void Parse_ValidRequest_WithoutBody_WithoutCRLFOnEnd()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\nHost: test.com");

        var expectedHeaders = new Dictionary<string, string>
        {
            { "Host", "test.com" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Get;
        ReadOnlyMemory<byte>? expectedBody = null;

        // Act
        var result = _parser.Parse(request).Value;
        WriteContextData(result);

        // Assert
        result.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Body.Should().BeNull();
        result.Body.Should().BeEquivalentTo(expectedBody);
    }

    [Fact]
    public void Parse_ValidRequest_WithSimpleBody()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("PUT / HTTP/1.1\r\nHost: test.com\r\nContent-Type: text/plain\r\nContent-Length: 13\r\n\r\nHello, World!");

        var expectedHeaders = new Dictionary<string, string>(3)
        {
            { "Host", "test.com" },
            { "Content-Type", "text/plain" },
            { "Content-Length", "13" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Put;
        var expectedBody = Encoding.UTF8.GetBytes("Hello, World!");

        // Act
        var result = _parser.Parse(request).Value;
        WriteContextData(result);

        // Assert
        result.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);
        
        ParseHeadersToStrings(result, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Body.Should().NotBeNull();
        result.Body.Value.ToArray().Should().BeEquivalentTo(expectedBody);
    }

    private static void ParseHeadersToStrings(HttpContext result, out Dictionary<string, string> actualHeaders)
    {
        actualHeaders = result.Headers.ToDictionary(
            h => Encoding.UTF8.GetString(h.Key.Span),
            h => Encoding.UTF8.GetString(h.Value.Span)
        );
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutHttpVersion()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET /\r\nHost: test.com");

        // Act
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutMethod()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("/ HTTP1.0\r\nHost: test.com");
        
        // Act
        var result = _parser.Parse(request);
        
        // Assert
        result.Success.Should().BeFalse();
        result.Exception.Should().BeOfType<ArgumentException>(); 
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutRoute()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET HTTP1.0\r\nHost: test.com");
        
        // Act
        var result = _parser.Parse(request);
        
        // Assert
        result.Success.Should().BeFalse();
        result.Exception.Should().BeOfType<ArgumentException>(); 
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutSpaces()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET/HTTP1.0\r\nHost: test.com");
        
        // Act 
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Exception.Should().BeOfType<ArgumentException>(); 
    }
    
    private void WriteContextData(HttpContext context)
    {
        _outputHelper.WriteLine($"The request route gained after parsing: {Encoding.UTF8.GetString(context.Route.Span)}");
        _outputHelper.WriteLine($"The request method gained after parsing: {Encoding.UTF8.GetString(context.Method.Span)}");
        _outputHelper.WriteLine(context.Body.HasValue
            ? $"Request body gained after parsing: {Encoding.UTF8.GetString(context.Body.Value.Span)}"
            : "Request does not have body");
        _outputHelper.WriteLine($"The request after parsing contains {context.Headers.Count}");
        _outputHelper.WriteLine($"Request headers gained after parsing: ");
        
        foreach (var header in context.Headers)
            _outputHelper.WriteLine($"{Encoding.UTF8.GetString(header.Key.Span)}: {Encoding.UTF8.GetString(header.Value.Span)}");
    }
}