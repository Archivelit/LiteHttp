using System.Net.Sockets;

using LiteHttp.Models;

namespace LiteHttp.Helpers;

public sealed class ConnectionContextFactory
{
    private long _nextId;
    
    public ConnectionContext Create(SocketAsyncEventArgs saea) => new(Interlocked.Increment(ref _nextId), saea);
}