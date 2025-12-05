namespace LiteHttp.Sockets.TcpSocket;

/// <summary>
/// Represents convenient socket model to work with tcp connections.
/// </summary>
internal sealed class TcpSocket : IDisposable
{
    /// <summary>
    /// Internal socket that <see cref="TcpSocket"/> works with
    /// </summary>
    private readonly Socket _internalSocket;
    /// <summary>
    /// An <see cref="TcpSocketReader"/> instance that used to receive data from the entire request.
    /// </summary>
    private readonly TcpSocketReader _socketReader = new();
    
    /// <summary>
    /// Creates <see cref="TcpSocket"/> instance.
    /// </summary>
    /// <param name="addressFamily">Specifies supported addresses that <see cref="TcpSocket"/> can use.</param>
    /// <param name="socketType">Specifies the type of socket that <see cref="TcpSocket"/> represents.</param>
    public TcpSocket(AddressFamily addressFamily = AddressFamily.InterNetwork,
        SocketType socketType = SocketType.Stream) =>
        _internalSocket = new Socket(addressFamily, socketType, ProtocolType.Tcp);

    /// <summary>
    /// Creates a <see cref="TcpSocket"/> instance that encapsulates an existing <see cref="Socket"/>.
    /// </summary>
    /// <param name="socket">
    /// The existing socket to be wrapped and used internally by the <see cref="TcpSocket"/> instance.
    /// </param>
    public TcpSocket(Socket socket) => _internalSocket = socket;
    
    public void Dispose() => _internalSocket.Dispose();
    
    /// <summary>
    /// Reads the data from entire request and writes them to <see cref="PipeReader"/> using <see cref="PipeWriter"/>. 
    /// </summary>
    /// <param name="pipe">Pipe used to store request for further processing.</param>
    /// <returns>A <see cref="Task"/> that represents asynchronous receive operation.</returns>
    public Task ReceiveAsync(Pipe pipe) =>
        _socketReader.ReceiveAsync(_internalSocket, pipe);
}