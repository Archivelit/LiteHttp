using System.Net.Sockets;
using System.Runtime.CompilerServices;

using LiteHttp.Models;

namespace LiteHttp.Helpers;

public sealed class ConnectionContextFactory
{
    private long _nextId;
    
    [SkipLocalsInit]
    public ConnectionContext Create(SocketAsyncEventArgs saea) => new(Interlocked.Increment(ref _nextId), saea);
}