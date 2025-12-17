using LiteHttp.Models.Events;

namespace LiteHttp.Listener;

public sealed partial class Listener
{
    public event Func<RequestReceivedEvent, CancellationToken, ValueTask>? RequestReceived;

    public void OnRequestReceived(RequestReceivedEvent connection, CancellationToken ct) =>
        RequestReceived?.Invoke(connection, ct);

    public void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        RequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        RequestReceived -= handler;
}