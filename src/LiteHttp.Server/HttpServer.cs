namespace LiteHttp.Server;

#pragma warning disable CS8618, CS4014
public sealed class HttpServer : IServer
{
    private readonly Listener.Listener _listener = new();
    private readonly RequestEventBus _eventBus = new();
	private readonly EndpointProvider _endpointProvider = new();
    
    private ReverseProxy _reverseProxy;
    private ServerWorker[]? _workerPool;
    
    public HttpServer() =>
        Initialize();

    public HttpServer(int workersCount)
    {
        _workerPool = new ServerWorker[workersCount];
        
        Initialize();
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        Task.Run(() =>_listener.StartListen(cancellationToken));
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var @event = await _eventBus.ConsumeAsync(cancellationToken);
            _reverseProxy.Proxy(@event, cancellationToken);
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

        _workerPool ??= new ServerWorker[1];
        
        for (int i = 0; i < _workerPool.Length; i++)
        {
            _workerPool[i] = new(_endpointProvider);
        }

        _reverseProxy = new ReverseProxy(_workerPool);
    }
}