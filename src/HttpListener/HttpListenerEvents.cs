namespace LiteHttp.HttpListener;

public sealed partial class HttpListener : IRequestPublisher
{
    public event Action<string>? OnRequestReceived;

    public void RaiseRequestReceived(string context) =>
        OnRequestReceived?.Invoke(context);

    public void SubscribeToRequestReceived(Action<string> handler) =>
        OnRequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Action<string> handler) =>
        OnRequestReceived -= handler;
}