namespace LiteHttp.Abstractions;

public interface IRequestPublisher
{
    event Action<string>? OnRequestReceived;
    void RaiseRequestReceived(string context);
    void SubscribeToRequestReceived(Action<string> handler);
    void UnsubscribeFromRequestReceived(Action<string> handler);
}