namespace LiteHttp.Listener;

public sealed partial class Listener : IRequestReceiver
{
    public event Func<RequestReceivedEvent, CancellationToken, Task>? OnRequestReceived;

    public void RaiseRequestReceived(RequestReceivedEvent connection, CancellationToken ct) =>
        OnRequestReceived?.Invoke(connection, ct);

    public void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, Task> handler) =>
        OnRequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, Task> handler) =>
        OnRequestReceived -= handler;
}