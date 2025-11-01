namespace LiteHttp.Abstractions;

public interface IReceiver
{
    Task<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct);
}