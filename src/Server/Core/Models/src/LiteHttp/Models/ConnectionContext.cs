namespace LiteHttp.Models;

public sealed class ConnectionContext
{
    public SocketAsyncEventArgs SocketEventArgs { get; init; }
    public HttpContext HttpContext { get; set; }
    public ulong Id { get; }

    public ConnectionContext(ulong id, SocketAsyncEventArgs saea)
    {
        Id = id;
        SocketEventArgs = saea;
    }
}
