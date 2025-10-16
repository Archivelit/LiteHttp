namespace LiteHttp.Abstractions;

public interface IServer
{
    Task Start(CancellationToken ct);
}