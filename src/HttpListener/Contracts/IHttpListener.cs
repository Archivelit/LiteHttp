namespace LiteHttp.HttpListener;

public interface IHttpListener
{
    Task StartListen(CancellationToken ct);
}
