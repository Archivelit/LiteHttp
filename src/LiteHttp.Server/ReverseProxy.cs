namespace LiteHttp.Server;

public class ReverseProxy(
    ServerWorker[] workerPool
    ) : IReverseProxy<RequestReceivedEvent>
{
    public readonly Channel<ServerWorker> _availableWorkers = Channel.CreateBounded<ServerWorker>(workerPool.Length);

    public async ValueTask Proxy(RequestReceivedEvent @event, CancellationToken ct)
    {
        var worker = await _availableWorkers.Reader.ReadAsync(ct).ConfigureAwait(false);

        worker?.HandleRequest(@event, ct);
    }

    public ValueTask PublishWorker(ServerWorker worker) =>
        _availableWorkers.Writer.WriteAsync(worker);
}