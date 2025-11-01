namespace LiteHttp.Abstractions;

public interface IReceiver
{
    ValueTask<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct);
}