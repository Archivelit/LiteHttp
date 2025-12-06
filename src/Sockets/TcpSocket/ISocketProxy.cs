namespace LiteHttp.Sockets.TcpSocket;

internal interface ISocketProxy
{
    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken);
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);

}
