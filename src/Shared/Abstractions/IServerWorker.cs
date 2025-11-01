namespace LiteHttp.Abstractions;

public interface IServerWorker
{
    ValueTask HandleEvent(RequestReceivedEvent @event, CancellationToken ct);
    void Initialize(IEndpointProvider endpointProvider);
}