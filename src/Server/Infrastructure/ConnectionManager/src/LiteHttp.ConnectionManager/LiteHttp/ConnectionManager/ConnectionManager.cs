using System.Collections.Concurrent;

namespace LiteHttp.ConnectionManager;

#nullable disable
public sealed class ConnectionManager
{
    private readonly DefaultObjectPool<SocketAsyncEventArgs> _saeaPool;
    private readonly ConcurrentDictionary<ulong, ConnectionContext> _connections;
    private ulong _nextId = 0;
    
    public ConnectionManager()
    {
        const int initObjectsCount = 50000;

        _connections = new ConcurrentDictionary<ulong, ConnectionContext>(-1, initObjectsCount);
        
        _saeaPool = new();
        ObjectPoolInitializationHelper<SocketAsyncEventArgs>.Initialize(initObjectsCount, _saeaPool, () =>
        {
            const int bufferSize = 4 * 1024; // 4 KB
            var saea = new SocketAsyncEventArgs();

            saea.Completed += IOCompleted;
            saea.SetBuffer(new byte[bufferSize], 0, bufferSize);

            return saea;
        });
    }

    public void ReceiveFrom(SocketAsyncEventArgs acceptEventArg)
    {
        var connection = acceptEventArg.AcceptSocket;

        if (_saeaPool.TryGet(out var saea))
        {
            saea.AcceptSocket = connection;

            bool willRaiseEvent = connection.ReceiveAsync(saea);
            
            if (!willRaiseEvent)
                ProcessReceive(saea);
        }
    }

    public void SendResponse(ConnectionContext connectionContext)
    {
        var socket = connectionContext.SocketEventArgs.AcceptSocket;
        bool willRaiseEvent = socket.SendAsync(connectionContext.SocketEventArgs);
        
        if (!willRaiseEvent)
            ProcessSend(connectionContext.SocketEventArgs);
    }

    // enable nullable to turn of CS8632 warning
#nullable enable
    private void IOCompleted(object? sender, SocketAsyncEventArgs saea)
    {
        switch(saea.LastOperation)
        {
            case SocketAsyncOperation.Receive:
                ProcessReceive(saea);
                break;
            case SocketAsyncOperation.Send:
                ProcessSend(saea);
                break;
        }
    }
#nullable disable

    private void ProcessSend(SocketAsyncEventArgs saea)
    {
        CloseConnection(saea);
        saea.AcceptSocket = null;
        saea.UserToken = null;
        
        _saeaPool.TryReturn(saea);
    }

    private void CloseConnection(SocketAsyncEventArgs saea)
    {
        var connectionContext = (ConnectionContext)saea.UserToken;
        
        if (!_connections.TryRemove(connectionContext.Id, out _))
            throw new InvalidOperationException($"Cannot remove task {connectionContext.Id}");
        
        saea.AcceptSocket.Shutdown(SocketShutdown.Both);
        saea.AcceptSocket.Close();
    }

    private void ProcessReceive(SocketAsyncEventArgs saea)
    {
        var connectionContext = new ConnectionContext(Interlocked.Increment(ref _nextId), saea);
        saea.UserToken = connectionContext;
        
        // REVIEW: not thread safe. Should be refactored to support multiple accept loops
        if (!_connections.TryAdd(connectionContext.Id, connectionContext))
            throw new InvalidOperationException($"Cannot add task {connectionContext.Id}");
        
        
        saea.SetBuffer(saea.Offset, saea.Offset + saea.BytesTransferred);

        OnDataReceived(connectionContext);
    }

    private event Action<ConnectionContext> DataReceived;
    private void OnDataReceived(ConnectionContext context) => DataReceived?.Invoke(context);

    public void SubscribeToDataReceived(Action<ConnectionContext> handler) => DataReceived += handler;
    public void UnsubscribeFromDataReceived(Action<ConnectionContext> handler) => DataReceived -= handler;
}
