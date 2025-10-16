namespace Server;

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

    // TODO: Add server flow
    public Task Start(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private void Initialize()
    {
        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);
        _router.SetMap(_endpoints);
    }
}