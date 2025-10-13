namespace LiteHttp.Abstractions;

public interface IEventBus<TEvent> 
    where TEvent : IEvent
{
    Task PublishAsync(TEvent @event, CancellationToken ct = default);
    Task<TEvent> ConsumeAsync(CancellationToken ct = default);
}