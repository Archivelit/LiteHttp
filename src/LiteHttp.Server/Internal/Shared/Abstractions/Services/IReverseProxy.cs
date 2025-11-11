namespace LiteHttp.Abstractions;

public interface IReverseProxy<TEvent> where TEvent : IEvent
{
    ValueTask Proxy(TEvent @event, CancellationToken ct);
}