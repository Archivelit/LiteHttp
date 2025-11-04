namespace LiteHttp.Abstractions;

public interface IListener
{
    ValueTask StartListen(CancellationToken ct);
    event Func<RequestReceivedEvent, CancellationToken, ValueTask>? RequestReceived;
    void OnRequestReceived(RequestReceivedEvent @event, CancellationToken ct);
    void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
    void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
}