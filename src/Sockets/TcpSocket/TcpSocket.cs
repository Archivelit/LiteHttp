namespace LiteHttp.Models;

/// <summary>
/// Represents convenient socket model to work with tcp connections.
/// </summary>
internal class TcpSocket : IDisposable
{
    /// <summary>
    /// Internal socket that <see cref="TcpSocket"/> works with
    /// </summary>
    private readonly Socket _internalSocket;
    
    /// <summary>
    /// Creates <see cref="TcpSocket"/> instance.
    /// </summary>
    /// <param name="addressFamily">Specifies supported addresses that <see cref="TcpSocket"/> can use.</param>
    /// <param name="socketType">Specifies the type of socket that <see cref="TcpSocket"/> represents.</param>
    public TcpSocket(AddressFamily addressFamily = AddressFamily.InterNetwork,
        SocketType socketType = SocketType.Stream) =>
        _internalSocket = new Socket(addressFamily, socketType, ProtocolType.Tcp);
    
    public void Dispose() => _internalSocket.Dispose();
}