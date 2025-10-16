using LiteHttp.Constants;

namespace LiteHttp.Server;

public sealed class HttpServer : IServer
{
    private readonly IListener _listener;
    private readonly IRequestSerializer _serializer;
    private readonly IRequestParser _parser;
    private readonly IRouter _router;
    private readonly IResponseGenerator _responseGenerator;
    private readonly IResponder _responder;
    private readonly IEventBus<RequestReceivedEvent> _eventBus;
    private readonly ILogger<HttpServer> _logger;
    private readonly Dictionary<(string, string), Func<IActionResult>> _endpoints = new();
    
    public HttpServer(IListener listener, IRequestSerializer serializer,  
        IRequestParser parser, IRouter router, IResponseGenerator responseGenerator, 
        IResponder responder, IEventBus<RequestReceivedEvent> eventBus, ILogger<HttpServer> logger)
    {
        _listener = listener;
        _serializer = serializer;
        _parser = parser;
        _router = router;
        _responseGenerator = responseGenerator;
        _responder = responder;
        _eventBus = eventBus;
        _logger = logger;

        Initialize();
    }
    
    public async Task Start(CancellationToken cancellationToken)
    {
        await _listener.StartListen(cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var @event = await _eventBus.ConsumeAsync(cancellationToken);
            
            var contextString = await _serializer.DeserializeFromConnectionAsync(@event.Connection, cancellationToken);
            var context = _parser.Parse(contextString);
            
            var action = _router.GetAction(context.Path, context.Method);
            
            if (action is null)
            {
                _logger.LogDebug("Unrecognized action");
                
                var notFoundResponse = _responseGenerator.Generate(new ActionResult(ResponseCode.NotFound), "HTTP/1.0");
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);
                
                continue;
            }

            var actionResult = action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = _responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
            else
                response = _responseGenerator.Generate(actionResult, "HTTP/1.0");

            await SendResponseAndDisposeConnection(@event.Connection, response);
        }
        
        async Task SendResponseAndDisposeConnection(Socket connection, string response)
        {
            await _responder.SendResponse(connection, response);
            
            connection.Close();
            connection.Dispose();            
        }
    }

    public void MapGet(string route, Func<IActionResult> action) =>
        _endpoints.Add((route, RequestMethods.Get), action);
    
    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpoints.Add((route, RequestMethods.Delete), action);

    public void MapPost(string route, Func<IActionResult> action) =>
        _endpoints.Add((route, RequestMethods.Post), action);

    public void MapPut(string route, Func<IActionResult> action) =>
        _endpoints.Add((route, RequestMethods.Put), action);
    
    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpoints.Add((route, RequestMethods.Patch), action);

    private void Initialize()
    {
        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);
        _router.SetMap(_endpoints);
    }
}