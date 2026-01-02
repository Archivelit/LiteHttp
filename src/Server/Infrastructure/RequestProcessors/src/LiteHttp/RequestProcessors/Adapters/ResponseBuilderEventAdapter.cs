namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ResponseBuilderEventAdapter
{
    private readonly ResponseBuilder _responseBuilder = new();
    
    public void Handle(object? sender, IActionResult actionResult)
    {
        var result = _responseBuilder.Build(actionResult);

        OnResponseBuilded(result);
    }

    private EventHandler<ReadOnlyMemory<byte>> ResponseBuidled;
    
    private void OnResponseBuilded(ReadOnlyMemory<byte> response) => ResponseBuidled?.Invoke(this, response);

    public void SubscriveResponseBuilded(EventHandler<ReadOnlyMemory<byte>> handler) => ResponseBuidled += handler;
    public void UnsubscriveResponseBuilded(EventHandler<ReadOnlyMemory<byte>> handler) => ResponseBuidled -= handler;
}