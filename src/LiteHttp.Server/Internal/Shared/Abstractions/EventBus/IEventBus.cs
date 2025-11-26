namespace LiteHttp.Abstractions;

public interface IEventBus<TEvent>
    where TEvent : IEvent
{
    public ValueTask PublishAsync(TEvent @event, CancellationToken ct = default);
    public ValueTask<TEvent> ConsumeAsync(CancellationToken ct = default);
}