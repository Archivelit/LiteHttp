namespace LiteHttp.Server;

#pragma warning disable CS8618
internal sealed class ServerWorker : IServerWorker, IDisposable
{
    private readonly Responder _responder = Responder.Instance;
    private readonly Router _router = new();
    private readonly Parser _parser = Parser.Instance;
    private readonly Receiver _receiver = Receiver.Instance;
    private readonly ResponseBuilder _reponseBuilder = new();
    // TODO: refactor to handle large requests and prevent unexpected errors

    public ServerWorker(IEndpointProvider endpointProvider, string address, int port) =>
        Initialize(endpointProvider);
    
    public void Initialize(IEndpointProvider endpointProvider) =>
        _router.SetProvider(endpointProvider);

    public void SetHostPort(int port) =>
        _reponseBuilder.Port = port;

    public void SetHostAddress(string address) =>
        _reponseBuilder.Address = address;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async Task HandleRequest(RequestReceivedEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var contextBytes = await _receiver.RecieveFromConnection(@event.Connection, cancellationToken).ConfigureAwait(false);
            
            var context = _parser.Parse(contextBytes);

            var action = _router.GetAction(context);

            if (action is null)
            {
                var notFoundResponse = _reponseBuilder.Build(ActionResultFactory.Instance.NotFound());
                
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse, cancellationToken).ConfigureAwait(false);

                return;
            }

            // TODO: add action execute module which will do work below
            
            var actionResult = action();

            ReadOnlyMemory<byte> response;

            if (actionResult is IActionResult<object> result)
                response = _reponseBuilder.Build(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty));
            else
                response = _reponseBuilder.Build(actionResult);

            await SendResponseAndDisposeConnection(@event.Connection, response, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // TODO: add exception logging
            var response = _reponseBuilder.Build(ActionResultFactory.Instance.InternalServerError());
            
            await SendResponseAndDisposeConnection(@event.Connection, response, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _reponseBuilder.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response, CancellationToken ct)
    {
        _ = await _responder.SendResponse(connection, response).ConfigureAwait(false);

        connection.Close();
        connection.Dispose();
    }
}