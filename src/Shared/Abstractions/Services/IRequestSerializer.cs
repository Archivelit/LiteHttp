namespace LiteHttp.Abstractions;

public interface IRequestSerializer
{
    Task<string> DeserializeFromConnectionAsync(Socket connection, CancellationToken ct);
}