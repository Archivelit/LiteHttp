namespace LiteHttp.Abstractions;

public interface IEventBus<TEvent> 
    where TEvent : IEvent
{
    void Publish(TEvent @event);
    TEvent Consume();
}