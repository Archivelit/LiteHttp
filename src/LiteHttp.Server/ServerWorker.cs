namespace LiteHttp.Server;

public class ServerWorker : IServerWorker
{
    private readonly IActionResultFabric _actionResultFabric;
    private readonly IResponder _responder;
    private readonly IResponseGenerator _responseGenerator;
    private readonly IRouter _router;
    private readonly IRequestParser _parser;
    private readonly IRequestSerializer _serializer;

    public ServerWorker(IActionResultFabric actionResultFabric, IResponder responder,
        IResponseGenerator responseGenerator, IRouter router, IRequestParser parser,
        IRequestSerializer serializer)
    {
        _actionResultFabric = actionResultFabric;
        _responder = responder;
        _responseGenerator = responseGenerator;
        _router = router;
        _parser = parser;
        _serializer = serializer;
    }

    public async Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        var contextString = await _serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
        var context = _parser.Parse(contextString);

        var action = _router.GetAction(context.Path, context.Method);

        if (action is null)
        {
            var notFoundResponse = _responseGenerator.Generate(_actionResultFabric.NotFound(), "HTTP/1.0");
            await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);

            return;
        }

        var actionResult = action();

        string response;

        if (actionResult is IActionResult<object> result)
            response = _responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
        else
            response = _responseGenerator.Generate(actionResult, "HTTP/1.0");

        await SendResponseAndDisposeConnection(@event.Connection, response);
    }

    public void Initialize(IEndpointProvider endpointProvider)
    {
        _router.SetProvider(endpointProvider);
    }

    private async Task SendResponseAndDisposeConnection(Socket connection, string response)
    {
        await _responder.SendResponse(connection, response);

        connection.Close();
        connection.Dispose();
    }
}