namespace LiteHttp.Abstractions;

public interface IListener
{
    public ValueTask StartListen(CancellationToken ct);
    public event Func<RequestReceivedEvent, CancellationToken, ValueTask>? RequestReceived;
    public void OnRequestReceived(RequestReceivedEvent @event, CancellationToken ct);
    public void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
    public void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler);
}