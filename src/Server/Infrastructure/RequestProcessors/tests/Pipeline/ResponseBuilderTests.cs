using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;
using ResponseBuilder = LiteHttp.RequestProcessors.PipeContext.ResponseBuilder;

namespace UnitTests.LiteHttp.RequestProcessors.Pipeline;

#nullable disable
public class ResponseBuilderTests
{
    private readonly ResponseBuilder _responseBuilder = new();

    [Fact]
    public async ValueTask Build_OkResponse_Should_BeSameAs_Expected()
    {
        // Arrange
        var actionResult = InternalActionResults.Ok();
        var expectedResponse = "HTTP/1.1 200 OK\r\nHost: " +
                       $"{AddressConstants.IPV4_LOOPBACK}" +
                       $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n\r\n";
        var requestPipe = new Pipe();

        var expectedHeaders = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>
        {
            {
                new ReadOnlyMemory<byte>(Encoding.ASCII.GetBytes("Host")), 
                new ReadOnlyMemory<byte>(Encoding.ASCII.GetBytes("test.com")) 
            }
        };

        var httpContext = new HttpContext(
            RequestMethodsAsBytes.Get, 
            Encoding.ASCII.GetBytes("/"), 
            expectedHeaders,
            null);

        // Act
        await _responseBuilder.Build(requestPipe, httpContext, actionResult);
        var actualResponse = await Read(requestPipe.Reader);

        // Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
    
    [Fact]
    public async ValueTask Build_OkResponse_WithBody_Should_BeSameAs_Expected()
    {
        // Arrange
        var actionResult = InternalActionResults.Ok();
        var expectedResponse = "HTTP/1.1 200 OK\r\nHost: " +
                               $"{AddressConstants.IPV4_LOOPBACK}" +
                               $":{AddressConstants.DEFAULT_SERVER_PORT}\r\n" +
                               $"{Encoding.ASCII.GetString(HeadersAsBytes.ContentType)}" +
                               $"{Encoding.ASCII.GetString(HeaderValuesAsBytes.ContentTextPlain)}" +
                               $"{Encoding.ASCII.GetString(HeadersAsBytes.ContentLength)}" +
                               $"13\r\n\r\n" + 
                               $"Hello, World!";
        var requestPipe = new Pipe();

        var contextHeaders = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>
        {
            {
                new ReadOnlyMemory<byte>(Encoding.ASCII.GetBytes("Host")),
                new ReadOnlyMemory<byte>(Encoding.ASCII.GetBytes("test.com"))
            },
            {
                new ReadOnlyMemory<byte>(HeadersAsBytes.ContentType), 
                new ReadOnlyMemory<byte>(HeaderValuesAsBytes.ContentTextPlain)
            },
            {
                new ReadOnlyMemory<byte>(HeadersAsBytes.ContentLength), 
                new ReadOnlyMemory<byte>(Encoding.ASCII.GetBytes(13.ToString()))
            }
        };

        var httpContext = new HttpContext(
            RequestMethodsAsBytes.Get, 
            Encoding.ASCII.GetBytes("/"), 
            contextHeaders,
            new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("Hello, World!")));

        // Act
        await _responseBuilder.Build(requestPipe, httpContext, actionResult);
        var actualResponse = await Read(requestPipe.Reader);

        // Assert
        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }


    private async ValueTask<string> Read(PipeReader reader)
    {
        var result = await reader.ReadAsync();
        

        if (SequenceMarshal.TryGetReadOnlyMemory(result.Buffer, out var memory))
            return Encoding.ASCII.GetString(memory.Span);

        var sequenceReader = new SequenceReader<byte>(result.Buffer);
        var stringBuilder = new StringBuilder();

        while (!sequenceReader.UnreadSequence.IsEmpty)
        {
            if (!sequenceReader.TryPeek(out var @byte))
                throw new Exception("Unexpected exception occured during reading sequence");
            stringBuilder.Append((char)@byte);
        }
        
        reader.AdvanceTo(result.Buffer.End);

        await reader.CompleteAsync();

        return stringBuilder.ToString();
    }
}
