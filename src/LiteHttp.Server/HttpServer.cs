namespace LiteHttp.Server;

public sealed class HttpServer : IServer
{
    private readonly Listener.Listener _listener;
    private readonly IServerWorker[] _workerPool;
    private readonly IEventBus<RequestReceivedEvent> _eventBus;
    private readonly ILogger<HttpServer> _logger;
	private readonly IEndpointProvider _endpointProvider;
    
    public HttpServer(Listener.Listener listener, IEventBus<RequestReceivedEvent> eventBus, 
        ILogger<HttpServer> logger, IEndpointProvider endpointProvider, int workersCount = 1)
    {
        _listener = listener;
        _eventBus = eventBus;
        _logger = logger;
        _endpointProvider = endpointProvider;
        _workerPool = new IServerWorker[workersCount];

        Initialize();
    }
    
    public async Task Start(CancellationToken cancellationToken)
    {
        await _listener.StartListen(cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            
        }
    }

    public void MapGet(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route, RequestMethods.Get, action);
    
    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route, RequestMethods.Delete, action);

    public void MapPost(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route, RequestMethods.Post, action);

    public void MapPut(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route, RequestMethods.Put, action);
    
    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route, RequestMethods.Patch, action);

    private void Initialize()
    {
        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);

        foreach (var worker in _workerPool)
            worker.Initialize(_endpointProvider);
    }
}