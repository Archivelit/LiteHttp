namespace LiteHttp.ConnectionManager;

internal sealed class ConnectionComparer : IEqualityComparer<ConnectionContext>
{
    public static ConnectionComparer Instance = new();

    public bool Equals(ConnectionContext? x, ConnectionContext? y) => x is not null 
        && y is not null 
        && x.Id.Equals(y.Id);

    public int GetHashCode([DisallowNull] ConnectionContext obj) => obj.Id.GetHashCode();
}