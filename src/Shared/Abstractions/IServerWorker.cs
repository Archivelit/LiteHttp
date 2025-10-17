namespace LiteHttp.Abstractions;

public interface IServerWorker
{
    WorkerStatus Status { get; }
    Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct);
    void Initialize(IEndpointProvider endpointProvider);
}