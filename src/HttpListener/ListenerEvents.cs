namespace LiteHttp.Listener;

public sealed partial class Listener : IRequestPublisher
{
    public event Action<Socket>? OnRequestReceived;

    public void RaiseRequestReceived(Socket connection) =>
        OnRequestReceived?.Invoke(connection);

    public void SubscribeToRequestReceived(Action<Socket> handler) =>
        OnRequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Action<Socket> handler) =>
        OnRequestReceived -= handler;
}