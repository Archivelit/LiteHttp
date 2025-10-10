namespace LiteHttp.Listener;

public sealed partial class Listener : IRequestReceiver
{
    public event Action<RequestReceivedEvent>? OnRequestReceived;

    public void RaiseRequestReceived(RequestReceivedEvent connection) =>
        OnRequestReceived?.Invoke(connection);

    public void SubscribeToRequestReceived(Action<RequestReceivedEvent> handler) =>
        OnRequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Action<RequestReceivedEvent> handler) =>
        OnRequestReceived -= handler;
}