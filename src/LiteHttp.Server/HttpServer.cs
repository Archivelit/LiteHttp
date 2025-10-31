﻿namespace LiteHttp.Server;

#pragma warning disable CS8618, CS4014
public sealed class HttpServer : IServer, IDisposable
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

    public async Task Start(CancellationToken cancellationToken = default)
    {
        try
        {
            InitializeWorkers();

            Task.Run(() => _listener.StartListen(cancellationToken));

            while (!cancellationToken.IsCancellationRequested)
            {
                var @event = await _eventBus.ConsumeAsync(cancellationToken).ConfigureAwait(false);
                _reverseProxy.Proxy(@event, cancellationToken);
            }
        }
        catch(OperationCanceledException)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _listener.Dispose();
    }

    public void MapGet(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(AsMemoryByteArray(route), RequestMethodsAsBytes.Get, action);
    
    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(AsMemoryByteArray(route), RequestMethodsAsBytes.Delete, action);

    public void MapPost(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(AsMemoryByteArray(route), RequestMethodsAsBytes.Post, action);

    public void MapPut(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(AsMemoryByteArray(route), RequestMethodsAsBytes.Put, action);
    
    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpointProvider.AddEndpoint(AsMemoryByteArray(route), RequestMethodsAsBytes.Patch, action);
    
    public void SetAddress(string address)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(address, nameof(address));
        
        var success = IPAddress.TryParse(address, out var iPAddress);
        
        if (!success)
            throw new InvalidOperationException($"Address {address} wrong formatted");

        _listener.SetIpAddress(iPAddress!);

        foreach(var worker in _workerPool)
        {
            worker.SetHostAddress(address);
        }
    }

    public void SetPort(int port)
    { 
        _listener.SetPort(port);
        
        foreach(var worker in _workerPool!)
        {
            worker.SetHostPort(port);
        }
    }
    
    private static ReadOnlyMemory<byte> AsMemoryByteArray(string s)
    {
        if (string.IsNullOrEmpty(s))
            return ReadOnlyMemory<byte>.Empty;

        var bytes = Encoding.UTF8.GetBytes(s);
        return new(bytes);
    }
    
    private void Initialize()
    {
        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);
        InitializeWorkers();
    }
    
    private void InitializeWorkers()
    {
        _workerPool ??= new ServerWorker[1];

        for (var i = 0; i < _workerPool.Length; i++)
        {
            _workerPool[i] = new(_endpointProvider, _listener.ListenerAddress.ToString(), _listener.ListenerPort);
        }

        _reverseProxy = new ReverseProxy(_workerPool);

        foreach(var worker in _workerPool)
        { 
            worker.WorkCompleted += _reverseProxy.PublishWorker;
            _reverseProxy.PublishWorker(worker);
        }
    }
}