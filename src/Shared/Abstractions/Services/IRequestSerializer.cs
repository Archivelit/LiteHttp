namespace LiteHttp.Abstractions;

public interface IRequestSerializer
{
    Task<Memory<byte>> DeserializeFromConnectionAsync(Socket connection, Memory<byte> buffer, CancellationToken ct);
}