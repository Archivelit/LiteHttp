namespace LiteHttp.Sockets.TcpSocket;

internal interface ISocketProxy : IDisposable
{
    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);
}
