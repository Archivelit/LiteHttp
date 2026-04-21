namespace LiteHttp.Pipeline;

public sealed class Pipeline
{
    private readonly IRouter _router;
    private readonly Parser _parser;
    private readonly ResponseBuilder _responseBuilder;
    private readonly ActionInvoker _actionInvoker;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    internal Pipeline(PipelineFactory factory)
    {
        _router = factory.RouterFactory();
        _parser = factory.ParserFactory();
        _responseBuilder = factory.ResponseBuilderFactory();
        _actionInvoker = factory.ActionInvokerFactory();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [SkipLocalsInit]
    public void ProcessRequest(ConnectionContext connectionContext)
    {
        Memory<byte> buffer = connectionContext.SocketEventArgs.Buffer;
        var parsingResult = _parser.Parse(buffer);

        if (!parsingResult.Success)
        {
            SendResponse(connectionContext, buffer, InternalActionResults.BadRequest());
            return;
        }

        var action = _router.GetAction(parsingResult.Value);

        if (action is null)
        {
            SendResponse(connectionContext, buffer, InternalActionResults.NotFound());
            return;
        }

        var executionResult = _actionInvoker.Execute(action);

        SendResponse(connectionContext, buffer, executionResult);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SendResponse(ConnectionContext connectionContext, Memory<byte> buffer, IActionResult actionResult)
    {
        int responseLength = _responseBuilder.Build(actionResult, buffer);
        connectionContext.SocketEventArgs.SetBuffer(0, responseLength);
        ThreadPool.UnsafeQueueUserWorkItem(OnExecuted, connectionContext, false);
    }

    private Action<ConnectionContext> _executed;
    private void OnExecuted(ConnectionContext response) => _executed?.Invoke(response);

    public void SubscribeToExecuted(Action<ConnectionContext> handler) => _executed += handler;
    public void UnsubscribeFromExecuted(Action<ConnectionContext> handler) => _executed -= handler;
}