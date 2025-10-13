namespace LiteHttp.Abstractions;

public interface IRequestReceiver
{
    event Func<RequestReceivedEvent, CancellationToken, Task>? OnRequestReceived;
    void RaiseRequestReceived(RequestReceivedEvent @event, CancellationToken ct);
    void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, Task> handler);
    void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, Task> handler);
}