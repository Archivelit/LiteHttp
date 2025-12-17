namespace LiteHttp.WorkerServices;

#pragma warning disable CS8618
public sealed class ServerWorker : IServerWorker, IDisposable
{
    private readonly Responder _responder = Responder.Instance;
    private readonly Router _router = new();
    private readonly Parser _parser = Parser.Instance;
    private readonly Receiver _receiver = Receiver.Instance;
    private readonly ResponseBuilder _responseBuilder = new();

    private ILogger<ServerWorker> _logger = NullLogger<ServerWorker>.Instance;
    // TODO: refactor to handle large requests and prevent unexpected errors

    public ServerWorker(IEndpointContext endpointContext, string address, int port, ILogger logger) =>
        Initialize(endpointContext: endpointContext, logger: logger, port: port, address: address);

    public void SetHostPort(int port) =>
        _responseBuilder.Port = port;

    public void SetHostAddress(string address) =>
        _responseBuilder.Address = address;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async Task HandleRequest(RequestReceivedEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var contextBytes = await _receiver.RecieveFromConnection(@event.Connection, cancellationToken).ConfigureAwait(false);

            if (!contextBytes.Success)
            {
                await SendResponseAndDisposeConnection(@event.Connection, _responseBuilder.Build(InternalActionResults.InternalServerError())).ConfigureAwait(false);
                return;
            }

            var context = _parser.Parse(contextBytes.Value);

            if (!context.Success)
            {
                await SendResponseAndDisposeConnection(@event.Connection, _responseBuilder.Build(InternalActionResults.InternalServerError())).ConfigureAwait(false);
                return;
            }

            var action = _router.GetAction(context.Value);

            if (action is null)
            {
                _logger.LogInformation($"Endpoint not found");
                var notFoundResponse = _responseBuilder.Build(InternalActionResults.NotFound());

                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse).ConfigureAwait(false);

                return;
            }

            // TODO: add action execute module which will do work below

            var actionResult = action();

            var response = actionResult is IActionResult<object> result
                ? _responseBuilder.Build(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty))
                : _responseBuilder.Build(actionResult);
            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured during processing request");

            var response = _responseBuilder.Build(InternalActionResults.InternalServerError());
            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
    }

    public void Dispose() => _responseBuilder.Dispose();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response)
    {
        _ = await _responder.SendResponse(connection, response).ConfigureAwait(false);

        connection.Shutdown(SocketShutdown.Both);
        connection.Close();
        connection.Dispose();
    }

    private void Initialize(IEndpointContext endpointContext, ILogger logger, int port, string address)
    {
        _router.SetContext(endpointContext);
        _logger = logger.ForContext<ServerWorker>();

        _responseBuilder.Address = address;
        _responseBuilder.Port = port;
    }
}