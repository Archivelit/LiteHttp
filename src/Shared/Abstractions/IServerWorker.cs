namespace LiteHttp.Abstractions;

public interface IServerWorker
{
    Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct);
    void Initialize(IEndpointProvider endpointProvider);
}