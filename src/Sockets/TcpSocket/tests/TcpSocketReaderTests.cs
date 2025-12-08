// These tests are only included in DEBUG builds.
// This is because the Socket abstraction uses SocketProxy.
#if DEBUG

namespace LiteHttp.Sockets.TcpSocketTests;

public class TcpSocketReaderTests
{
    private readonly TcpSocketReader _tcpSocketReader = new();

    [Fact]
    public async ValueTask ReceiveAsync_RegularRequest_Should_WriteToPipe_ExpectedRequest()
    {
        // Arrange
        var socket = new TestScoketProxy();
        var pipe = new Pipe();
        var expectedRequest = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");
        
        socket.SetReceiveData(expectedRequest);
        
        // Act
        await _tcpSocketReader.ReceiveAsync(socket, pipe);

        // Assert
        var reader = pipe.Reader;
        var requestBuilder = new List<byte>(); // Used to accumulate received bytes

        while (reader.TryRead(out var result))
        {
            if (result.IsCompleted)
                break;

            requestBuilder.AddRange(result.Buffer.ToArray());
            reader.AdvanceTo(result.Buffer.End);
        }

        reader.Complete();

        var actualRequest = requestBuilder.ToArray();

        actualRequest.Should().BeEquivalentTo(expectedRequest);
    }
}
#endif