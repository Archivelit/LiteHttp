namespace HttpListener;

public interface IHttpListener
{
    Task ListenAsync(CancellationToken ct);
}
