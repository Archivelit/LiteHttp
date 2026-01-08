namespace LiteHttp.Models;

public sealed class ConnectionContext
{
    public SocketAsyncEventArgs SocketEventArgs { get; init; }
    public HttpContext HttpContext { get; set; }
    public long Id { get; }
    public long BytesReceived { get; private set; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    
    public ConnectionContext(long id, SocketAsyncEventArgs saea)
    {
        Id = id;
        SocketEventArgs = saea;
    }

    public void IncrementBytesReceived(long bytesReceived) => BytesReceived += bytesReceived;
}
