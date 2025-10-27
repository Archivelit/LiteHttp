namespace LiteHttp.Server;

public class ServerWorker : IServerWorker
{
    private readonly ActionResultFabric _actionResultFabric = new();
    private readonly Responder _responder = new();
    private readonly ResponseGenerator _responseGenerator = new();
    private readonly Router _router = new();
    private readonly RequestParser _parser = new();
    private readonly RequestSerializer _serializer = new();

    public WorkerStatus Status { get; private set; } = WorkerStatus.Waiting;
    
    public ServerWorker(IEndpointProvider endpointProvider) =>
        Initialize(endpointProvider);
    
    public void Initialize(IEndpointProvider endpointProvider)
    {
        _router.SetProvider(endpointProvider);
    }
    
    public async Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        Status = WorkerStatus.Working;
        
        var contextString = await _serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
        var context = _parser.Parse(contextString);

        var action = _router.GetAction(context.Path, context.Method);

        if (action is null)
        {
            var notFoundResponse = _responseGenerator.Generate(_actionResultFabric.NotFound(), "HTTP/1.0");
            await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);
            
            Status = WorkerStatus.Waiting;
            
            return;
        }

        var actionResult = action();

        string response;

        if (actionResult is IActionResult<object> result)
            response = _responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
        else
            response = _responseGenerator.Generate(actionResult, "HTTP/1.0");

        await SendResponseAndDisposeConnection(@event.Connection, response);
        
        Status = WorkerStatus.Waiting;
    }

    private async Task SendResponseAndDisposeConnection(Socket connection, string response)
    {
        await _responder.SendResponse(connection, response);

        connection.Close();
        connection.Dispose();
    }
}