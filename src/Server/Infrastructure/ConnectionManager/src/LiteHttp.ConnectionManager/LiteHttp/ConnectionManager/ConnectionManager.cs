// This implementation is partially inspired by ASP.NET Core Kestrel server
// Original source: https://github.com/dotnet/aspnetcore 
// License: MIT
// Modifications: "Walk" logic and connection management adapted for our ConcurrentDictionary setup.
//
// The rest of the code is written without any inspiration, any similarities are purely coincidental.

using System.Collections.Concurrent;

using LiteHttp.Heartbeat;

namespace LiteHttp.ConnectionManager;

#nullable disable
#pragma warning disable CS8632
public sealed class ConnectionManager : IHeartbeatHandler, IDisposable
{
    private const int MinimalReceiveSpeed = 1024; // 1 KB/s 
    private static readonly TimeSpan Second = TimeSpan.FromSeconds(1);
    
    private readonly DefaultObjectPool<SocketAsyncEventArgs> _saeaPool;
    private readonly ConcurrentDictionary<long, ConnectionContext> _connections;
    private long _nextId = 0;
    
    public ConnectionManager()
    {
        const int initObjectsCount = 50000;

        _connections = new ConcurrentDictionary<long, ConnectionContext>(-1, initObjectsCount);
        
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
    
    public void OnHeartbeat()
    {
        var now = DateTime.UtcNow;
        
        foreach (var kvp in _connections)
        {
            var connection = kvp.Value;
            
            var lifetime =  now - connection.CreatedAtUtc;
            if (lifetime < Second) continue;

            var speed = connection.BytesReceived / lifetime.TotalSeconds;
            if (speed < MinimalReceiveSpeed)
                CloseConnection(connection.SocketEventArgs);
        }
    }
    
    // TODO: Create ConnectionContext in the Accept-loop via factory shared by all
    // accept-loops, so each accept-loop owns its acceptEventArg.
    // Then queue the receive operation with ThreadPool.UnsafeQueueUserWorkItem to
    // decouple accept-loop from request processing. Ensure that adding to
    // the global _connections dictionary is refactored or synchronized to safely
    // support multiple accept-loops without races.
    public void ReceiveFrom(SocketAsyncEventArgs acceptEventArg)
    {
        var connection = acceptEventArg.AcceptSocket;

        if (!_saeaPool.TryGet(out var saea)) return;
        
        saea.AcceptSocket = connection;
        
        var connectionContext = new ConnectionContext(Interlocked.Increment(ref _nextId), saea);
        saea.UserToken = connectionContext;
        
        // REVIEW: not thread safe. Should be refactored to support multiple accept loops
        if (!_connections.TryAdd(connectionContext.Id, connectionContext))
            throw new InvalidOperationException($"Cannot add task {connectionContext.Id}");
        
        bool willRaiseEvent = connection!.ReceiveAsync(saea);
            
        if (!willRaiseEvent)
            ThreadPool.UnsafeQueueUserWorkItem(ProcessReceive, saea, false);
    }

    public void SendResponse(ConnectionContext connectionContext)
    {
        var socket = connectionContext.SocketEventArgs.AcceptSocket;
        bool willRaiseEvent = socket.SendAsync(connectionContext.SocketEventArgs);
        
        if (!willRaiseEvent)
            ProcessSend(connectionContext.SocketEventArgs);
    }
    
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

    private void ProcessSend(SocketAsyncEventArgs saea)
    {
        CloseConnection(saea);
        saea.AcceptSocket = null;
        saea.UserToken = null;
        saea.SetBuffer(0, saea.Buffer.Length);
        
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
        var connectionContext = (ConnectionContext)saea.UserToken;
        
        connectionContext.IncrementBytesReceived(saea.BytesTransferred);
        
        saea.SetBuffer(saea.Offset, saea.Offset + saea.BytesTransferred);
        
        OnDataReceived(connectionContext);
    }

    private event Action<ConnectionContext> DataReceived;
    private void OnDataReceived(ConnectionContext context) => DataReceived?.Invoke(context);

    public void SubscribeToDataReceived(Action<ConnectionContext> handler) => DataReceived += handler;
    public void UnsubscribeFromDataReceived(Action<ConnectionContext> handler) => DataReceived -= handler;

    public void Dispose()
    {
        // REVIEW: uncomment line below when object pool will implement IDisposable
        // _saeaPool.Dispose();
    }
}
