namespace LiteHttp.Server;

public class ServerWorker : IServerWorker
{
    private readonly ActionResultFactory _actionResultFactory = new();
    private readonly Responder _responder = new();
    private readonly Router _router = new();
    private readonly RequestParser _parser = new();
    private readonly RequestSerializer _serializer = new();
    private readonly ResponseGenerator _responseGenerator;

    public WorkerStatus Status { get; private set; } = WorkerStatus.Waiting;
    
    public ServerWorker(IEndpointProvider endpointProvider, string address, int port)
    {
        _responseGenerator = new(address, port);

        Initialize(endpointProvider);
    }
    
    public void Initialize(IEndpointProvider endpointProvider) =>
        _router.SetProvider(endpointProvider);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        Status = WorkerStatus.Working;
        
        try
        {
            var contextString = await _serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
            var context = _parser.Parse(contextString);

            var action = _router.GetAction(context.Path, context.Method);

            if (action is null)
            {
                var notFoundResponse = _responseGenerator.Generate(_actionResultFactory.NotFound(), HttpVersions.HTTP_1_1);
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);

                return;
            }

            var actionResult = action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = _responseGenerator.Generate(result, HttpVersions.HTTP_1_1, result.Result.ToString());
            else
                response = _responseGenerator.Generate(actionResult, HttpVersions.HTTP_1_1);

            await SendResponseAndDisposeConnection(@event.Connection, response);
        }
        catch (Exception)
        {
            // TODO: add exception logging

            var response = _responseGenerator.Generate(_actionResultFactory.InternalServerError(), HttpVersions.HTTP_1_1);
            await SendResponseAndDisposeConnection(@event.Connection, response);
        }
        finally
        {
            Status = WorkerStatus.Waiting;
        }
    }

    private async Task SendResponseAndDisposeConnection(Socket connection, string response)
    {
        await _responder.SendResponse(connection, response);

        connection.Close();
        connection.Dispose();
    }
}