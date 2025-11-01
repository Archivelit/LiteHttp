namespace LiteHttp.Abstractions;

public interface IEventBus<TEvent> 
    where TEvent : IEvent
{
    ValueTask PublishAsync(TEvent @event, CancellationToken ct = default);
    ValueTask<TEvent> ConsumeAsync(CancellationToken ct = default);
}