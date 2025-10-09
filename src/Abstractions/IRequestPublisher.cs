namespace LiteHttp.Abstractions;

public interface IRequestPublisher
{
    event Action<Socket>? OnRequestReceived;
    void RaiseRequestReceived(Socket context);
    void SubscribeToRequestReceived(Action<Socket> handler);
    void UnsubscribeFromRequestReceived(Action<Socket> handler);
}