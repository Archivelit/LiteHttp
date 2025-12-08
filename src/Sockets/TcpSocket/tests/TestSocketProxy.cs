namespace LiteHttp.Sockets.TcpSocketTests;

internal sealed class TestSocketProxy : ISocketProxy
{
    private readonly Queue<byte[]> _receiveQueue = new();

    public void Dispose() { }
    
    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        var request = _receiveQueue.Dequeue();
        request.CopyTo(buffer);
        return new(buffer.Length);
    }

    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => 
        new(buffer.Length);

    public void SetReceiveData(byte[] data) => _receiveQueue.Enqueue(data);
}
