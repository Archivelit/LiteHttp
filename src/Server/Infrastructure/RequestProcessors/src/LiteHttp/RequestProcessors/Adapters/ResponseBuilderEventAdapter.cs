using LiteHttp.Models.LiteHttp.Models;

namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ResponseBuilderEventAdapter
{
    private readonly ResponseBuilder _responseBuilder = new();
    
    public void Handle(ConnectionContext context, IActionResult actionResult)
    {
        var result = _responseBuilder.Build(actionResult);
        context.Buffer = MemoryMarshal.AsMemory(result);

        OnResponseBuilded(context);
    }

    private Action<ConnectionContext> ResponseBuidled;
    
    private void OnResponseBuilded(ConnectionContext response) => ResponseBuidled?.Invoke(response);

    public void SubscriveResponseBuilded(Action<ConnectionContext> handler) => ResponseBuidled += handler;
    public void UnsubscriveResponseBuilded(Action<ConnectionContext> handler) => ResponseBuidled -= handler;
}