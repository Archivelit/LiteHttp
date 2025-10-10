using System.Collections.Concurrent;
using LiteHttp.Abstractions;
using LiteHttp.Models.Events;

namespace EventBus;

public abstract class RequestEventBus : IEventBus<RequestReceivedEvent>
{
    private ConcurrentQueue<RequestReceivedEvent> _queue = new();
    
    public void Publish(RequestReceivedEvent @event)
    {
        _queue.Enqueue(@event);
    }
    
    // TODO: Add event consuming logic
    public RequestReceivedEvent Consume()
    {
        throw new NotImplementedException();
    }
}