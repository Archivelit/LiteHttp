namespace LiteHttp.Server;

#pragma warning disable CS8618
public class ServerWorker : IServerWorker
{
    private readonly ActionResultFactory _actionResultFactory = new();
    private readonly Responder _responder = new();
    private readonly Router _router = new();
    private readonly RequestParser _parser = new();
    private readonly RequestSerializer _serializer = new();
    private readonly ResponseGenerator _responseGenerator = new();

    public ServerWorker(IEndpointProvider endpointProvider, string address, int port) =>
        Initialize(endpointProvider);
    
    public void Initialize(IEndpointProvider endpointProvider) =>
        _router.SetProvider(endpointProvider);

    public void SetHostPort(int port) =>
        _responseGenerator.Port = port;

    public void SetHostAddress(string address) =>
        _responseGenerator.Address = address;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async Task HandleEvent(RequestReceivedEvent @event, CancellationToken ct)
    {
        try
        {
            // TODO: refactor to handle large requests and prevent unexpected errors
            using var owner = MemoryPool<byte>.Shared.Rent(4096);
            var buffer = owner.Memory;
            
            var contextBytes = await _serializer.DeserializeFromConnectionAsync(@event.Connection, buffer, ct).ConfigureAwait(false);
            var context = _parser.Parse(contextBytes);

            var action = _router.GetAction(context);

            if (action is null)
            {
                var notFoundResponse = _responseGenerator.Generate(_actionResultFactory.NotFound());
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse).ConfigureAwait(false);

                return;
            }

            var actionResult = action();

            ReadOnlyMemory<byte> response;
            
            // TODO: add action execute module which will do work below
            
            if (actionResult is IActionResult<object> result)
                response = _responseGenerator.Generate(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty));
            else
                response = _responseGenerator.Generate(actionResult);

            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // TODO: add exception logging

            var response = _responseGenerator.Generate(_actionResultFactory.InternalServerError());
            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
        finally
        {
            await WorkCompleted.Invoke(this);
        }
    }

    private async Task SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response)
    {
        await _responder.SendResponse(connection, response).ConfigureAwait(false);

        connection.Close();
        connection.Dispose();
    }

    public event Func<ServerWorker, Task> WorkCompleted;
}