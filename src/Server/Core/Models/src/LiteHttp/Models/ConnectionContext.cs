namespace LiteHttp.Models;

[StructLayout(LayoutKind.Sequential)]
public sealed class ConnectionContext
{
    public HttpContext HttpContext { get; set; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    public SocketAsyncEventArgs SocketEventArgs { get; init; }
    public long Id { get; }
    public long BytesReceived { get; private set; }
    
    public ConnectionContext(long id, SocketAsyncEventArgs saea)
    {
        Id = id;
        SocketEventArgs = saea;
    }

    public void IncrementBytesReceived(long bytesReceived) => BytesReceived += bytesReceived;
}
