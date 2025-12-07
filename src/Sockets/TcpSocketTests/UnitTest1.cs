// These tests are only included in DEBUG builds.
// This is because the Socket abstraction uses SocketProxy.
#if DEBUG
using Moq;
namespace LiteHttp.Sockets.TcpSocketTests;

public class TcpSocketReaderTests
{
    private readonly TcpSocketReader _tcpSocketReader = new();

    [Fact]
    public void ReceiveAsync_RegularRequest_Should_WriteToPipe_ExpectedRequest()
    {
        // Arrange
        //var mockedProxy = new Mock<ISocketProxy>();
        //mockedProxy
        //    .Setup(mockedProxy => mockedProxy.ReceiveAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
        //    .Returns<>();

        // Act

        // Assert
    }
}
#endif