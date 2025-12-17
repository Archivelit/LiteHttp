namespace LiteHttp.Sockets.TcpSocket;

#if RELEASE
/// <summary>
/// Represents convenient socket model to work with tcp connections.
/// </summary>
public sealed class TcpSocket : IDisposable
{
    /// <summary>
    /// Internal socket that <see cref="TcpSocket"/> works with
    /// </summary>
    private readonly Socket _internalSocket;
    /// <summary>
    /// An <see cref="TcpSocketReader"/> instance used to receive data from the entire request.
    /// </summary>
    private readonly TcpSocketReader _socketReader = new();
    /// <summary>
    /// An <see cref="TcpSocketWriter"/> instance used to send response.
    /// </summary>
    private readonly TcpSocketWriter _socketWriter = new();

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
    /// The existing socket to be wrapped and used internally by the <see cref="TcpSocket"/> instance. The
    /// <see cref="ProtocolType"/> value has to be Tcp 
    /// </param>
    public TcpSocket(Socket socket) => _internalSocket = socket;
    
    /// <summary>
    /// Dispose encapsulated socket. Use only when you are sure that socket is not needed anymore.
    /// </summary>
    public void Dispose() => _internalSocket.Dispose();
    
    /// <summary>
    /// Reads the data from entire request and writes them to provided <see cref="Pipe"/>. 
    /// </summary>
    /// <param name="pipe">Pipe used to store request data for further processing.</param>
    /// <returns>A <see cref="Task"/> that represents asynchronous receive operation.</returns>
    public Task ReceiveAsync(Pipe pipe, CancellationToken ct = default) =>
        _socketReader.ReceiveAsync(_internalSocket, pipe, ct);

    /// <summary>
    /// Reads the response from provided <see cref="Pipe"/> and sends it to client.
    /// </summary>
    /// <param name="pipe">Pipe with response stored in it.</param>
    /// <returns>A <see cref="Task"/> that represents asynchronous send operation.</returns>
    public Task SendAsync(Pipe pipe, CancellationToken ct = default) =>
        _socketWriter.SendAsync(_internalSocket, pipe, ct);
}

#else

/// <summary>
/// Represents convenient socket model to work with tcp connections. Uses <see cref="ISocketProxy"/> internally for easier testing.
/// </summary>
/// <remarks>
/// This class is intended for tests and is not intended to be exposed publicly.
/// </remarks>
public sealed class TcpSocket : IDisposable
{
    /// <summary>
    /// Internal wrapped socket that <see cref="TcpSocket"/> works with
    /// </summary>
    private readonly ISocketProxy _internalSocket;
    /// <summary>
    /// An <see cref="TcpSocketReader"/> instance used to receive data from the entire request.
    /// </summary>
    private readonly TcpSocketReader _socketReader = new();
    /// <summary>
    /// An <see cref="TcpSocketWriter"/> instance used to send response.
    /// </summary>
    private readonly TcpSocketWriter _socketWriter = new();

    /// <summary>
    /// Creates <see cref="TcpSocket"/> instance.
    /// </summary>
    /// <param name="addressFamily">Specifies supported addresses that <see cref="TcpSocket"/> can use.</param>
    /// <param name="socketType">Specifies the type of socket that <see cref="TcpSocket"/> represents.</param>
    public TcpSocket(AddressFamily addressFamily = AddressFamily.InterNetwork,
        SocketType socketType = SocketType.Stream) =>
        _internalSocket = new SocketProxy(new Socket(addressFamily, socketType, ProtocolType.Tcp));

    /// <summary>
    /// Creates a <see cref="TcpSocket"/> instance that encapsulates a wrapped <see cref="Socket"/> instance.
    /// </summary>
    /// <param name="socket">
    /// The existing socket to be wrapped and used internally by the <see cref="TcpSocket"/> instance. The
    /// <see cref="ProtocolType"/> value has to be Tcp 
    /// </param>
    internal TcpSocket(ISocketProxy socket) => _internalSocket = socket;

    /// <summary>
    /// Dispose encapsulated socket. Use only when you are sure that socket is not needed anymore.
    /// </summary>
    public void Dispose() => _internalSocket.Dispose();

    /// <summary>
    /// Reads the data from entire request and writes them to provided <see cref="Pipe"/>. 
    /// </summary>
    /// <param name="pipe">Pipe used to store request data for further processing.</param>
    /// <returns>A <see cref="Task"/> that represents asynchronous receive operation.</returns>
    public Task ReceiveAsync(Pipe pipe, CancellationToken ct = default) =>
        _socketReader.ReceiveAsync(_internalSocket, pipe, ct);

    /// <summary>
    /// Reads the response from provided <see cref="Pipe"/> and sends it to client.
    /// </summary>
    /// <param name="pipe">Pipe with response stored in it.</param>
    /// <returns>A <see cref="Task"/> that represents asynchronous send operation.</returns>
    public Task SendAsync(Pipe pipe, CancellationToken ct = default) =>
        _socketWriter.SendAsync(_internalSocket, pipe, ct);
}
#endif