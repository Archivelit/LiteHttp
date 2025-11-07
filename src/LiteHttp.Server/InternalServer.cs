namespace LiteHttp.Server;

internal sealed class InternalServer : IServer, IDisposable
{
    private readonly Listener.Listener _listener = new();
    private readonly RequestEventBus _eventBus = new();
	private readonly EndpointProvider _endpointProvider = new();
    private readonly ILogger<InternalServer> _logger = NullLogger<InternalServer>.Instance;

    private ServerWorker[]? _workerPool;
    
    public InternalServer() =>
        Initialize();

    public InternalServer(int workersCount)
    {
        _workerPool = new ServerWorker[workersCount];
        
        Initialize();
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting server");
        
        try
        {
            List<Task> tasks = new(_workerPool!.Length + 1); // +1 for listener task

            var listenerTask = Task.Run(async () => await _listener.StartListen(cancellationToken));

            tasks.Add(listenerTask);

            foreach (var worker in _workerPool)
            {
                var workerTask = Task.Run(async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var @event = await _eventBus.ConsumeAsync(cancellationToken).ConfigureAwait(false);
                        await worker.HandleRequest(@event, cancellationToken).ConfigureAwait(false);
                    }
                });
                tasks.Add(workerTask);
            }

            await Task.WhenAll(tasks);
        }
        catch(OperationCanceledException)
        {
            Dispose();
        }
        
        _logger.LogInformation($"Server work ended");
    }

    public void Dispose()
    {
        _listener.Dispose();
    }
    
    public void MapGet(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Get, action);
    
    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Delete, action);

    public void MapPost(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Post, action);

    public void MapPut(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Put, action);

    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Patch, action);
    
    public void SetAddress(string address)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(address, nameof(address));
    
        var success = IPAddress.TryParse(address, out var iPAddress);
        
        _logger.LogInformation($"Setting server address to {address}...");
        
        if (!success)
            throw new InvalidOperationException($"Address {address} wrong formatted");

        _listener.SetIpAddress(iPAddress!);

        foreach(var worker in _workerPool!)
        {
            worker.SetHostAddress(address);
        }

        _logger.LogInformation($"Server address set to {address} successfully.");
    }

    public void SetPort(int port)
    { 
        _logger.LogInformation($"Setting server port to {port}...");

        try
        {
            _listener.SetPort(port);

            foreach (var worker in _workerPool!)
            {
                worker.SetHostPort(port);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while setting server port to {port}");
            throw;
        }
    }
    
    private void Initialize()
    {
        _logger.LogInformation($"Initializing InternalServer...");

        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);
        InitializeWorkers();
    
        _logger.LogInformation($"InternalServer initialized successfully.");
    }
    
    private void InitializeWorkers()
    {
        _logger.LogInformation($"Initializing server workers...");

        _workerPool ??= new ServerWorker[1];

        for (var i = 0; i < _workerPool.Length; i++)
        {
            _workerPool[i] = new(_endpointProvider, _listener.ListenerAddress.ToString(), _listener.ListenerPort);
        }

        _logger.LogInformation($"Server workers initialized successfully.");
    }
}