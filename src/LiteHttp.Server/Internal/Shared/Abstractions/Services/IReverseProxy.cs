namespace LiteHttp.Abstractions;

public interface IReverseProxy<TEvent> where TEvent : IEvent
{
    public ValueTask Proxy(TEvent @event, CancellationToken ct);
}