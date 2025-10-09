namespace LiteHttp.Abstractions;

public interface IHttpListener
{
    Task StartListen(CancellationToken ct);
}
