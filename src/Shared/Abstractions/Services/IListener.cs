namespace LiteHttp.Abstractions;

public interface IListener
{
    ValueTask StartListen(CancellationToken ct);
    event Func<RequestReceivedEvent, CancellationToken, ValueTask>? OnRequestReceived;
    void RaiseRequestReceived(RequestReceivedEvent @event, CancellationToken ct);
    void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
    void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
}