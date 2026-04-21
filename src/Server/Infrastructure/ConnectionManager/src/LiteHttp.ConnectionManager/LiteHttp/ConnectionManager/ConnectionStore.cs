using System.Collections.Concurrent;

using LiteHttp.ConnectionManager.Abstractions;
using LiteHttp.Helpers;

namespace LiteHttp.ConnectionManager;

// Used to store active connections and manage them (close, timeout, etc.).
// The motivation to extract Walker service to avoid iterating over connections in ConnectionManager itself,
// and instead delegate this responsibility to a separate service, which will be called in Heartbeat.
// This way we can keep ConnectionManager focused on connection management (creation, closing, etc.) and have a
// separate service that will be responsible for "walking" through connections and performing necessary checks
// (like timeouts, etc.). This separation of concerns can lead to cleaner code and better maintainability.
internal sealed class ConnectionStore : IConnectionStore
{
    private readonly ConcurrentDictionary<long, ConnectionContext> _connections;
    private readonly ConnectionContextFactory _connectionContextFactory = new();

    public ConnectionStore()
    {
        const int initialCapacity = 10000;

        _connections = new(-1, initialCapacity);
    }

    public bool TryCloseConnection(ConnectionContext connection) => _connections.TryRemove(connection.Id, out _);

    public ConnectionContext InitializeConnection(SocketAsyncEventArgs saea)
    {
        var connectionContext = _connectionContextFactory.Create(saea);
        if (!_connections.TryAdd(connectionContext.Id, connectionContext)) 
            ;
        return connectionContext;
    }
}
