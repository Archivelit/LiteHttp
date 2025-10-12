namespace LiteHttp.Abstractions;

public interface IRequestProcessor
{
    Task ProcessConnection(Socket connection, CancellationToken ct);
}