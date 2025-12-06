namespace LiteHttp.Sockets.TcpSocket;

internal sealed class SocketProxy : ISocketProxy
{
    private readonly Socket _socket;

    public SocketProxy(Socket socket) => _socket = socket;

    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => _socket.ReceiveAsync(buffer, cancellationToken);
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => _socket.SendAsync(buffer, cancellationToken);
    public void Dispose() => _socket.Dispose();
}
