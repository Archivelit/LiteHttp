namespace LiteHttp.Abstractions;

using System.Collections.Concurrent;

public interface IServerWorker
{
    Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct);
    void Initialize(IEndpointProvider endpointProvider);
}