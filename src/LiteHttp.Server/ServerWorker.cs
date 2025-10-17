namespace LiteHttp.Server;

public class ServerWorker(
    IActionResultFabric actionResultFabric,
    IResponder responder,
    IResponseGenerator responseGenerator,
    IRouter router,
    IRequestParser parser,
    IRequestSerializer serializer)
    : IServerWorker
{
    public WorkerStatus Status { get; private set; } = WorkerStatus.Waiting;
        
    public void Initialize(IEndpointProvider endpointProvider)
    {
        router.SetProvider(endpointProvider);
    }
    
    public async Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        Status = WorkerStatus.Working;
        
        var contextString = await serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
        var context = parser.Parse(contextString);

        var action = router.GetAction(context.Path, context.Method);

        if (action is null)
        {
            var notFoundResponse = responseGenerator.Generate(actionResultFabric.NotFound(), "HTTP/1.0");
            await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);

            return;
        }

        var actionResult = action();

        string response;

        if (actionResult is IActionResult<object> result)
            response = responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
        else
            response = responseGenerator.Generate(actionResult, "HTTP/1.0");

        await SendResponseAndDisposeConnection(@event.Connection, response);
        
        Status = WorkerStatus.Waiting;
    }

    private async Task SendResponseAndDisposeConnection(Socket connection, string response)
    {
        await responder.SendResponse(connection, response);

        connection.Close();
        connection.Dispose();
    }
}