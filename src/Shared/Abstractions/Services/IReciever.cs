namespace LiteHttp.Abstractions;

public interface IReciever
{
    Task<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct);
}