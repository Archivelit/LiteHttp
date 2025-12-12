using Parser = LiteHttp.RequestProcessors.Pipeline.Parser;
using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace UnitTests.LiteHttp.RequestProcessors.Pipeline;

#nullable disable
public class ParserTests
{
    private readonly Parser _parser = new();
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;

    [Fact(Skip = "Not completed yet")]
    public async Task Parse_ValidRequest_WithoutBody_WithCRLFOnEnd_ShouldReturn_HttpContext()
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

        var requestPipe = new Pipe();
        var writer = requestPipe.Writer;
        
        TestContext.Current.TestOutputHelper?.WriteLine("Writing request to pipe");
        
        TestContext.Current.TestOutputHelper?.WriteLine("Writing to pipe buffer");
        
        var buffer = writer.GetMemory(request.Length);

        request.CopyTo(buffer.Span);
        writer.Advance(request.Length);
        
        TestContext.Current.TestOutputHelper?.WriteLine("Written to pipe buffer");
            
        TestContext.Current.TestOutputHelper?.WriteLine("Flushing");
        
        var flushResult = await writer.FlushAsync(TestContext.Current.CancellationToken);

        await writer.CompleteAsync();
    
        TestContext.Current.TestOutputHelper?.WriteLine("Request written to pipe");
        
        // Act
        var result = await _parser.Parse(requestPipe);
        WriteContextData(result.Value);

        // Assert
        result.Value.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Value.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result.Value, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Value.Body.Should().BeNull();
    }

    /*[Fact]
    public void Parse_ValidRequest_WithoutBody_WithoutCRLFOnEnd_ShouldReturn_HttpContext()
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

    [Fact] public void Parse_ValidRequest_WithSimpleBody_ShouldReturn_HttpContext()
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

    [Fact]
    public void Parse_InvalidRequest_WithoutHttpVersion_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET /\r\nHost: test.com");

        // Act
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutMethod_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("/ HTTP1.0\r\nHost: test.com");

        // Act
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutRoute_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET HTTP1.0\r\nHost: test.com");

        // Act
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public void Parse_InvalidRequest_WithoutSpaces_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = Encoding.UTF8.GetBytes("GET/HTTP1.0\r\nHost: test.com");

        // Act 
        var result = _parser.Parse(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }
    */

    private void WriteContextData(HttpContext context)
    {
        _outputHelper.WriteLine($"The request route gained after parsing: {Encoding.UTF8.GetString(context.Route.Span)}");
        _outputHelper.WriteLine($"The request method gained after parsing: {Encoding.UTF8.GetString(context.Method.Span)}");
        _outputHelper.WriteLine(context.Body.HasValue
            ? $"Request body gained after parsing: {Encoding.UTF8.GetString(context.Body.Value.FirstSpan)}"
            : "Request does not have body");
        //_outputHelper.WriteLine($"The request after parsing contains {context.Headers.Count}");
        _outputHelper.WriteLine($"Request headers gained after parsing: ");

        foreach (var header in context.Headers)
            _outputHelper.WriteLine($"{Encoding.UTF8.GetString(header.Key.Span)}: {Encoding.UTF8.GetString(header.Value.Span)}");
    }
    
    private static void ParseHeadersToStrings(HttpContext result, out Dictionary<string, string> actualHeaders) => 
        actualHeaders = result.Headers.ToDictionary(
            h => Encoding.UTF8.GetString(h.Key.Span),
            h => Encoding.UTF8.GetString(h.Value.Span)
        );
}