namespace LiteHttp.Abstractions;

public interface IReceiver
{
    public Task<Result<Memory<byte>>> RecieveFromConnection(Socket connection, CancellationToken ct);
}