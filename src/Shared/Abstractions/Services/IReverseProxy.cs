namespace LiteHttp.Abstractions;

public interface IReverseProxy<TEvent> where TEvent : IEvent
{
    void Proxy(TEvent @event, CancellationToken ct);
}