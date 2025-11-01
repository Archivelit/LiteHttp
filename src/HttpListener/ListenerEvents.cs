namespace LiteHttp.Listener;

public sealed partial class Listener : IListener, IDisposable
{
    public event Func<RequestReceivedEvent, CancellationToken, ValueTask>? OnRequestReceived;

    public void RaiseRequestReceived(RequestReceivedEvent connection, CancellationToken ct) =>
        OnRequestReceived?.Invoke(connection, ct);

    public void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        OnRequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        OnRequestReceived -= handler;
}