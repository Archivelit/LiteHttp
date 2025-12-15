using Parser = LiteHttp.RequestProcessors.Pipeline.Parser;
using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace UnitTests.LiteHttp.RequestProcessors.Pipeline;

#nullable disable
public class ParserTests
{
    private readonly Parser _parser = new();
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper;
    private readonly Pipe _requestPipe = new Pipe(); 

    [Fact]
    public async Task Parse_ValidRequest_WithoutBody_WithCRLFOnEnd_ShouldReturn_HttpContext()
    {
        // Arrange
        var request = "GET / HTTP/1.1\r\nHost: test.com\r\n\r\n";

        var expectedHeaders = new Dictionary<string, string>
        {
            { "Host", "test.com" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Get;

        await FillPipeWith(request);

        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        if(!result.Success)
        {
            _outputHelper.WriteLine(result.Error.Value.ErrorMessage);
            return;
        }
        WriteContextData(result.Value);

        result.Value.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Value.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result.Value, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Value.Body.Should().BeNull();
    }
    
    [Fact]
    public async ValueTask Parse_ValidRequest_WithoutBody_WithoutCRLFOnEnd_ShouldReturn_HttpContext()
    {
        // Arrange
        var request = "GET / HTTP/1.1\r\nHost: test.com\r\n";

        var expectedHeaders = new Dictionary<string, string>
        {
            { "Host", "test.com" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Get;

        await FillPipeWith(request);

        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        if(!result.Success)
        {
            _outputHelper.WriteLine(result.Error.Value.ErrorMessage);
            return;
        }
        WriteContextData(result.Value);

        result.Value.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Value.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result.Value, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Value.Body.Should().BeNull();
    }

    [Fact] public async ValueTask Parse_ValidRequest_WithSimpleBody_ShouldReturn_HttpContext()
    {
        // Arrange
        var request = "PUT / HTTP/1.1\r\nHost: test.com\r\nContent-Type: text/plain\r\nContent-Length: 13\r\n\r\nHello, World!";

        var expectedHeaders = new Dictionary<string, string>(3)
        {
            { "Host", "test.com" },
            { "Content-Type", "text/plain" },
            { "Content-Length", "13" }
        };

        var expectedRoute = Encoding.UTF8.GetBytes("/");
        var expectedMethod = RequestMethodsAsBytes.Put;
        var expectedBody = Encoding.UTF8.GetBytes("Hello, World!");

        await FillPipeWith(request);
        
        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        if(!result.Success)
        {
            _outputHelper.WriteLine(result.Error.Value.ErrorMessage);
            return;
        }
        WriteContextData(result.Value);
        
        result.Value.Method.Span.ToArray().Should().BeEquivalentTo(expectedMethod);
        result.Value.Route.Span.ToArray().Should().BeEquivalentTo(expectedRoute);

        ParseHeadersToStrings(result.Value, out var actualHeaders);
        actualHeaders.Should().BeEquivalentTo(expectedHeaders);

        result.Value.Body.Should().NotBeNull();
        result.Value.Body.Value.ToArray().Should().BeEquivalentTo(expectedBody);
    }

    [Fact]
    public async ValueTask Parse_InvalidRequest_WithoutHttpVersion_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = "GET /\r\nHost: test.com";
    
        await FillPipeWith(request);
        
        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public async ValueTask Parse_InvalidRequest_WithoutMethod_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = "/ HTTP1.0\r\nHost: test.com";

        await FillPipeWith(request);
        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public async ValueTask Parse_InvalidRequest_WithoutRoute_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = "GET HTTP1.0\r\nHost: test.com";

        await FillPipeWith(request);
        // Act
        var result = await _parser.Parse(_requestPipe);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Value.Should().NotBeNull().And.BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    [Fact]
    public async ValueTask Parse_InvalidRequest_WithoutSpaces_ShouldReturn_ResultWithError_InvalidRequestSyntax()
    {
        // Arrange
        var request = "GET/HTTP1.0\r\nHost: test.com";

        await FillPipeWith(request);
        // Act 
        var result = await _parser.Parse(_requestPipe);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.Value.ErrorCode.Should().Be(ParserErrors.InvalidRequestSyntax);
    }

    private async ValueTask FillPipeWith(string request)
    {
        var requestBytes = Encoding.ASCII.GetBytes(request);
        var writer = _requestPipe.Writer;

        var buffer = writer.GetSpan(requestBytes.Length);
        requestBytes.CopyTo(buffer);
        writer.Advance(requestBytes.Length);

        await writer.FlushAsync();
        await writer.CompleteAsync();
    }

    private void WriteContextData(HttpContext context)
    {
        _outputHelper.WriteLine($"The request route gained after parsing: {Encoding.UTF8.GetString(context.Route.Span)}");
        _outputHelper.WriteLine($"The request method gained after parsing: {Encoding.UTF8.GetString(context.Method.Span)}");
        _outputHelper.WriteLine(context.Body.HasValue
            ? $"Request body gained after parsing" // : {Encoding.UTF8.GetString(context.Body.Value.FirstSpan)}
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