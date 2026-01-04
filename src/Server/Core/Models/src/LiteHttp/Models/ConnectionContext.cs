namespace LiteHttp.Models;

public sealed class ConnectionContext
{
    public SocketAsyncEventArgs SocketEventArgs { get; init; }
    public HttpContext HttpContext { get; set; }

    public ConnectionContext(SocketAsyncEventArgs saea) => SocketEventArgs = saea;
}
