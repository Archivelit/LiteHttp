namespace LiteHttp.Abstractions;

public interface IReceiver
{
    public ValueTask<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct);
}