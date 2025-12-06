namespace LiteHttp.Sockets.TcpSocket;

/// <summary>
/// Provides a proxy implementation of the ISocketProxy interface that delegates socket operations to an underlying
/// System.Net.Sockets.Socket instance.
/// </summary>
/// <remarks>SocketProxy enables abstraction over socket operations, allowing for easier testing and substitution
/// of socket behavior. All methods forward calls directly to the wrapped Socket object. This class is intended for
/// primarily for unit testing or mocking and is not thread-safe; callers are responsible for ensuring 
/// thread safety when accessing instances concurrently.</remarks>
internal sealed class SocketProxy : ISocketProxy
{
    private readonly Socket _socket;

    /// <summary>
    /// Initializes a new instance of the SocketProxy class that wraps the specified Socket.
    /// </summary>
    /// <param name="socket">The Socket instance to be proxied. Cannot be null.</param>
    public SocketProxy(Socket socket) => _socket = socket;

    /// <summary>
    /// Receives data asynchronously from the connected socket and writes it into the specified buffer.
    /// </summary>
    /// <param name="buffer">The memory buffer that receives the incoming data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the receive operation.</param>
    /// <returns>A ValueTask representing the asynchronous receive operation. The result contains the number of bytes received,
    /// which may be zero if the remote endpoint has closed the connection.</returns>
    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => _socket.ReceiveAsync(buffer, cancellationToken);
    /// <summary>
    /// Asynchronously sends data from the specified buffer to the connected remote endpoint.
    /// </summary>
    /// <param name="buffer">The buffer containing the data to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A ValueTask representing the asynchronous send operation. The result is the number of bytes sent to the remote
    /// endpoint.</returns>
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => _socket.SendAsync(buffer, cancellationToken);
    /// <summary>
    /// Disposes the proxy and the underlying socket.
    /// </summary>
    public void Dispose() => _socket.Dispose();
}
