namespace LiteHttp.Abstractions;

public interface IReceiver
{
    public ValueTask<Result<Memory<byte>>> RecieveFromConnection(Socket connection, CancellationToken ct);
}