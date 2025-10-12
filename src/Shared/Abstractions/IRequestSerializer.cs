namespace LiteHttp.Abstractions;

public interface IRequestSerializer
{
    Task DeserializeFromConnectionAsync(Socket connection, CancellationToken ct);
}