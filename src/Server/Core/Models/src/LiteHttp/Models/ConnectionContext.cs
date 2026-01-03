namespace LiteHttp.Models.LiteHttp.Models;

public sealed class ConnectionContext
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Memory<byte> Buffer { get; set; }
    public SocketAsyncEventArgs SocketEventArgs { get; init; }
    public HttpContext HttpContext { get; set; }

    public ConnectionContext(SocketAsyncEventArgs saea, Memory<byte> buffer)
    {
        Buffer = buffer;
        SocketEventArgs = saea;
    }
}
