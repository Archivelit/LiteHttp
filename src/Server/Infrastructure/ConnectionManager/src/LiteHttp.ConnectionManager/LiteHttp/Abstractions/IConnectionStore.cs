namespace LiteHttp.ConnectionManager.Abstractions;

public interface IConnectionStore
{
    public bool TryCloseConnection(ConnectionContext connection);
    public ConnectionContext InitializeConnection(SocketAsyncEventArgs saea);
}
