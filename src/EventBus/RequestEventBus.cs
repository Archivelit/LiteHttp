using System.Collections.Concurrent;
using LiteHttp.Abstractions;
using LiteHttp.Models.Events;
using Microsoft.Extensions.Logging;

namespace LiteHttp.EventBus;

public sealed class RequestEventBus(
    ILogger<RequestEventBus> logger
    ) : IEventBus<RequestReceivedEvent>
{
    private ConcurrentQueue<RequestReceivedEvent> _queue = new();
    
    public void Publish(RequestReceivedEvent @event)
    {
        _queue.Enqueue(@event);
        logger.LogDebug($"RequestReceivedEvent published");
    }
    
    public RequestReceivedEvent? Consume()
    {
        logger.LogDebug("RequestReceivedEvent consumed");
        return _queue.TryDequeue(out var @event) ? @event : null;
    }
}