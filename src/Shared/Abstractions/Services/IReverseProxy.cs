namespace LiteHttp.Abstractions;

public interface IReverseProxy<TEvent> where TEvent : IEvent
{
    Task Proxy(TEvent @event, CancellationToken ct);
}