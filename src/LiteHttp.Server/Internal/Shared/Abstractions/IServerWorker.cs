namespace LiteHttp.Abstractions;

public interface IServerWorker
{
    public Task HandleRequest(RequestReceivedEvent @event, CancellationToken ct);
}