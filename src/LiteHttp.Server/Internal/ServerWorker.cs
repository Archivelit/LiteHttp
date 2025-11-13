using System.Text.Json;
using System.Text.Json.Serialization;

namespace LiteHttp.Server;

#pragma warning disable CS8618
internal sealed class ServerWorker : IServerWorker, IDisposable
{
    private readonly Responder _responder = Responder.Instance;
    private readonly Router _router = new();
    private readonly Parser _parser = Parser.Instance;
    private readonly Receiver _receiver = Receiver.Instance;
    private readonly ResponseBuilder _responseBuilder = new();

    private ILogger<ServerWorker> _logger = NullLogger<ServerWorker>.Instance; 
    // TODO: refactor to handle large requests and prevent unexpected errors

    public ServerWorker(IEndpointProvider endpointProvider, string address, int port) =>
        Initialize(endpointProvider);
    
    public void Initialize(IEndpointProvider endpointProvider) =>
        _router.SetProvider(endpointProvider);

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

            var action = _router.GetAction(context);

            if (action is null)
            {
                _logger.LogInformation($"Endpoint not found");
                var notFoundResponse = _responseBuilder.Build(ActionResultFactory.Instance.NotFound());
                
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse, cancellationToken).ConfigureAwait(false);

                return;
            }

            // TODO: add action execute module which will do work below
            
            var actionResult = action();

            ReadOnlyMemory<byte> response;

            if (actionResult is IActionResult<object> result)
                response = _responseBuilder.Build(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty));
            else
                response = _responseBuilder.Build(actionResult);

            await SendResponseAndDisposeConnection(@event.Connection, response, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured during processing request");
            
            var response = _responseBuilder.Build(ActionResultFactory.Instance.InternalServerError());
            await SendResponseAndDisposeConnection(@event.Connection, response, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _responseBuilder.Dispose();
    }

    internal void SetLogger(ILogger logger)
    {
        _logger = logger.ForContext<ServerWorker>();
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async ValueTask SendResponseAndDisposeConnection(Socket connection, ReadOnlyMemory<byte> response, CancellationToken ct)
    {
        _logger.LogDebug($"Sending response");
        
        _ = await _responder.SendResponse(connection, response).ConfigureAwait(false);
        
        _logger.LogInformation($"Response sent successfully");
        
        connection.Close();
        connection.Dispose();
    }
}