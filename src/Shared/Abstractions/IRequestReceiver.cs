namespace LiteHttp.Abstractions;

public interface IRequestReceiver
{
    event Action<RequestReceivedEvent>? OnRequestReceived;
    void RaiseRequestReceived(RequestReceivedEvent @event);
    void SubscribeToRequestReceived(Action<RequestReceivedEvent> handler);
    void UnsubscribeFromRequestReceived(Action<RequestReceivedEvent> handler);
}