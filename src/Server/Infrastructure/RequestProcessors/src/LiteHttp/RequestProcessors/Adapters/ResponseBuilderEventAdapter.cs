namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ResponseBuilderEventAdapter
{
    private readonly SaeaResponseBuilder _responseBuilder = new();
    
    public void BuildResponse(ConnectionContext context, IActionResult actionResult)
    {
        var written = _responseBuilder.Build(actionResult, context.SocketEventArgs.Buffer);
        
        context.SocketEventArgs.SetBuffer(0, written);

        OnResponseBuilded(context);
    }

    private Action<ConnectionContext> ResponseBuidled;
    private void OnResponseBuilded(ConnectionContext response) => ResponseBuidled?.Invoke(response);

    public void SubscriveResponseBuilded(Action<ConnectionContext> handler) => ResponseBuidled += handler;
    public void UnsubscriveResponseBuilded(Action<ConnectionContext> handler) => ResponseBuidled -= handler;
}