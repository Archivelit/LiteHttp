namespace LiteHttp.Abstractions;

public interface IListener
{
    Task StartListen(CancellationToken ct);
}