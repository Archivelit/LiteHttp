namespace LiteHttp.Abstractions;

public interface IServerWorker
{
    Task HandleRequest(RequestReceivedEvent @event, CancellationToken ct);
    void Initialize(IEndpointProvider endpointProvider);
}