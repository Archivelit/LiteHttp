// These tests are only included in DEBUG builds.
// This is because the Socket abstraction uses SocketProxy.
#if DEBUG

namespace LiteHttp.Sockets.TcpSocketTests;

public class TcpSocketReaderTests
{
    private readonly TcpSocketReader _tcpSocketReader = new();
    
    [Fact]
    public async Task ReceiveAsync_RegularRequest_Should_WriteToPipe_ExpectedRequest()
    {
        // Arrange
        var socket = new TestSocketProxy();
        var expectedRequest = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");
        
        var pipe = new Pipe();
        var reader = pipe.Reader;
        
        socket.SetReceiveData(expectedRequest);
        
        // Act
        await _tcpSocketReader.ReceiveAsync(socket, pipe);

        // Assert
        var requestBuilder = new List<byte>(expectedRequest.Length); // Used to accumulate received bytes
        
        while (true)
        {
            reader.TryRead(out var result);
            var buffer = result.Buffer;
            
            requestBuilder.AddRange(buffer.ToArray());
            
            reader.AdvanceTo(buffer.Start, buffer.End);
            
            if (result.IsCompleted)
                break;
        }
        
        await reader.CompleteAsync();
        var actualRequest = requestBuilder.ToArray();
        
        actualRequest.Should().BeEquivalentTo(expectedRequest);
    }
}

#endif