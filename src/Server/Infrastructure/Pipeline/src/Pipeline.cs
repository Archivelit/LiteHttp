namespace LiteHttp.Pipeline;

#nullable disable
public sealed class Pipeline
{
    private readonly IRouter _router;
    private readonly Parser _parser;
    private readonly ResponseBuilder _responseBuilder;
    private readonly ActionInvoker _actionInvoker;
    
    internal Pipeline(PipelineFactory factory)
    {
        _router = factory.RouterFactory();
        _parser = factory.ParserFactory();
        _responseBuilder = factory.ResponseBuilderFactory();
        _actionInvoker = factory.ActionInvokerFactory();
    }
    
    [SkipLocalsInit]
    public void ProcessRequest(ConnectionContext connectionContext)
    {
        Memory<byte> buffer = connectionContext.SocketEventArgs.Buffer;
        var parsingResult = _parser.Parse(buffer);
        int responseLength;
        
        if (!parsingResult.Success)
        {
            responseLength = _responseBuilder.Build(InternalActionResults.BadRequest(), buffer);
            connectionContext.SocketEventArgs.SetBuffer(0, responseLength);
            ThreadPool.UnsafeQueueUserWorkItem(OnExecuted, connectionContext, false);
            return;
        }

        var action = _router.GetAction(parsingResult.Value);

        if (action is null)
        {
            responseLength = _responseBuilder.Build(InternalActionResults.NotFound(), buffer);
            connectionContext.SocketEventArgs.SetBuffer(0, responseLength);
            ThreadPool.UnsafeQueueUserWorkItem(OnExecuted, connectionContext, false);
            return;
        }

        var executionResult = _actionInvoker.Execute(action);

        responseLength = _responseBuilder.Build(executionResult, buffer);
        connectionContext.SocketEventArgs.SetBuffer(0, responseLength);
        ThreadPool.UnsafeQueueUserWorkItem(OnExecuted, connectionContext, false);
    }
    
    private Action<ConnectionContext> _executed;
    private void OnExecuted(ConnectionContext response) => _executed?.Invoke(response);

    public void SubscribeToExecuted(Action<ConnectionContext> handler) => _executed += handler;
    public void UnsubscribeFromExecuted(Action<ConnectionContext> handler) => _executed -= handler;
}