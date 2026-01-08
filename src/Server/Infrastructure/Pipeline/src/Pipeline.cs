namespace LiteHttp.Pipeline;

public sealed class Pipeline
{
    private readonly IRouter _router;
    private readonly Parser _parser;
    private readonly SaeaResponseBuilder _responseBuilder;
    private readonly Executor _executor;
    
    internal Pipeline(PipelineFactory factory)
    {
        _router = factory.RouterFactory();
        _parser = factory.ParserFactory();
        _responseBuilder = factory.ResponseBuilderFactory();
        _executor = factory.ExecutorFactory();
    }

    public void ProcessRequest(ConnectionContext connectionContext)
    {
        Memory<byte> buffer = connectionContext.SocketEventArgs.Buffer;
        var parsingResult = _parser.Parse(buffer);
        
        if (!parsingResult.Success)
        {
            _responseBuilder.Build(InternalActionResults.BadRequest(), buffer);
            return;
        }

        var action = _router.GetAction(parsingResult.Value);

        if (action is null)
        {
            _responseBuilder.Build(InternalActionResults.NotFound(), buffer);
            return;
        }

        var executionResult = _executor.Execute(action);

        _responseBuilder.Build(executionResult, buffer);

        ThreadPool.UnsafeQueueUserWorkItem(OnExecuted, connectionContext, false);
    }
    
    private Action<ConnectionContext> _executed;
    private void OnExecuted(ConnectionContext response) => _executed?.Invoke(response);

    public void SubscribeToExecuted(Action<ConnectionContext> handler) => _executed += handler;
    public void UnsubscribeFromExecuted(Action<ConnectionContext> handler) => _executed -= handler;
}