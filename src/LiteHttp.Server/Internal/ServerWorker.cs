namespace LiteHttp.Server;

#pragma warning disable CS8618
internal sealed class ServerWorker : IServerWorker, IDisposable
{
    private readonly Responder _responder = Responder.Instance;
    private readonly Router _router = new();
    private readonly Parser _parser = Parser.Instance;
    private readonly Receiver _receiver = Receiver.Instance;
    private readonly ResponseBuilder _responseBuilder = new();

    private ILimitsProvider _limitsProvider { get; set; }
    private ILogger<ServerWorker> _logger = NullLogger<ServerWorker>.Instance;
    // TODO: refactor to handle large requests and prevent unexpected errors

    public ServerWorker(IEndpointContext endpointContext, string address, int port, ILogger logger, ILimitsProvider limits) =>
        Initialize(endpointContext: endpointContext, logger: logger, port: port, address: address, limitsProvider: limits);

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

            var context = _parser.Parse(contextBytes);

            if (!context.Success)
                await SendResponseAndDisposeConnection(@event.Connection, _responseBuilder.Build(ActionResultFactory.Instance.InternalServerError())).ConfigureAwait(false);

            var action = _router.GetAction(context.Value);

            if (action is null)
            {
                _logger.LogInformation($"Endpoint not found");
                var notFoundResponse = _responseBuilder.Build(ActionResultFactory.Instance.NotFound());

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

            var response = _responseBuilder.Build(ActionResultFactory.Instance.InternalServerError());
            await SendResponseAndDisposeConnection(@event.Connection, response).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _responseBuilder.Dispose();
        _requestBuffer.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response)
    {
        _logger.LogDebug($"Sending response");

        _ = await _responder.SendResponse(connection, response).ConfigureAwait(false);

        _logger.LogInformation($"Response sent successfully");

        connection.Close();
        connection.Dispose();
    }

    private void Initialize(IEndpointContext endpointContext, ILogger logger, int port, string address, ILimitsProvider limitsProvider)
    {
        _router.SetContext(endpointContext);
        _logger = logger.ForContext<ServerWorker>();

        _responseBuilder.Address = address;
        _responseBuilder.Port = port;
        _limitsProvider = limitsProvider;
    }
}