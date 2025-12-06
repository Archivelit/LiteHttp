namespace LiteHttp.Sockets.TcpSocket;

internal class SocketProxy : ISocketProxy
{
    private readonly Socket _socket;

    public SocketProxy(Socket socket) => _socket = socket;

    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken) => _socket.ReceiveAsync(buffer, cancellationToken);
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken) => _socket.SendAsync(buffer, cancellationToken);
}
