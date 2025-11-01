namespace LiteHttp.Server;

#pragma warning disable CS8618
public class ServerWorker : IServerWorker, IDisposable
{
    private readonly ActionResultFactory _actionResultFactory = new();
    private readonly Responder _responder = new();
    private readonly Router _router = new();
    private readonly Parser _parser = new();
    private readonly Receiver _serializer = new();
    private readonly ResponseBuilder _responseGenerator = new();
    // TODO: refactor to handle large requests and prevent unexpected errors

    public ServerWorker(IEndpointProvider endpointProvider, string address, int port) =>
        Initialize(endpointProvider);
    
    public void Initialize(IEndpointProvider endpointProvider) =>
        _router.SetProvider(endpointProvider);

    public void SetHostPort(int port) =>
        _responseGenerator.Port = port;

    public void SetHostAddress(string address) =>
        _responseGenerator.Address = address;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async ValueTask HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        try
        {
            var contextBytes = await _serializer.RecieveFromConnection(@event.Connection, ct).ConfigureAwait(false);
            var context = _parser.Parse(contextBytes);

            var action = _router.GetAction(context);

            if (action is null)
            {
                var notFoundResponse = _responseGenerator.Build(_actionResultFactory.NotFound());
                
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse).ConfigureAwait(false);

                return;
            }

            var actionResult = action();

            ReadOnlyMemory<byte> response;

            // TODO: add action execute module which will do work below

            if (actionResult is IActionResult<object> result)
                response = _responseGenerator.Build(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty));
            else
                response = _responseGenerator.Build(actionResult);

            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // TODO: add exception logging
            var response = _responseGenerator.Build(_actionResultFactory.InternalServerError());
            
            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
        finally
        {
            WorkCompleted.Invoke(this);
        }
    }

    public void Dispose()
    {
        _responseGenerator.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response)
    {
        _ = await _responder.SendResponse(connection, response).ConfigureAwait(false);

        connection.Close();
        connection.Dispose();
    }

    public event Func<ServerWorker, ValueTask> WorkCompleted;
}