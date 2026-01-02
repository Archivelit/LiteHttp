using LiteHttp.Constants;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;
using LiteHttp.Models;
using LiteHttp.Routing;
using LiteHttp.WorkerServices;

namespace LiteHttp.Server;

internal sealed class InternalServer : IServer, IDisposable
{
    private readonly Listener.Listener _listener = new();
    private readonly RequestEventBus _eventBus = new();
    
    private readonly ILogger<InternalServer> _logger = NullLogger<InternalServer>.Instance;
    private ServerWorker[]? _workerPool;
    private readonly IEndpointProviderConfiguration _endpointProviderConfiguration;

    internal InternalServer(int workersCount, IPAddress? address = null, int port = AddressConstants.DEFAULT_SERVER_PORT, ILogger? logger = null)
    {
        _workerPool = new ServerWorker[workersCount];
        logger ??= NullLogger.Instance;
        address ??= IPAddress.Loopback;
        _endpointProviderConfiguration = new EndpointProviderConfiguration();

        Initialize(logger: logger, port: port, address: address);
    }

    /// <inheritdoc/>
    public async Task Start(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting server");

        _endpointProviderConfiguration.Freeze();

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

            _logger.LogInformation($"Server started successfully. Waiting for incoming connections.");

            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException)
        {
            Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured during server work");
            Dispose();
        }

        _logger.LogInformation($"Server work ended");
    }

    public void Dispose()
    {
        _listener.Dispose();

        if (_workerPool == null)
            return;

        foreach (var worker in _workerPool)
            worker.Dispose();
    }

    /// <inheritdoc/>
    public void MapGet(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Get, action);

    /// <inheritdoc/>
    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Delete, action);

    /// <inheritdoc/>
    public void MapPost(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Post, action);

    /// <inheritdoc/>
    public void MapPut(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Put, action);

    /// <inheritdoc/>
    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Patch, action);

    /// <inheritdoc/>
    [Obsolete("Must be used builder instead")]
    public void SetAddress(string address)
    {
        ArgumentException.ThrowIfNullOrEmpty(address, nameof(address));

        var success = IPAddress.TryParse(address, out var iPAddress);

        _logger.LogInformation($"Setting server address to {address}...");

        if (!success)
            throw new InvalidOperationException($"Address {address} wrong formatted");

        _listener.ListenerAddress = iPAddress!;

        foreach (var worker in _workerPool!)
            worker.SetHostAddress(address);

        _logger.LogInformation($"Server address set to {address} successfully.");
    }

    /// <summary>
    /// Sets the server's IP address for incoming connections.
    /// </summary>
    /// <remarks>Calling this method updates the server's listening address and notifies all worker instances
    /// of the new address. Prefer using the builder pattern for configuration in new code.</remarks>
    /// <param name="address">The IP address to assign to the server. Cannot be null.</param>
    [Obsolete("Must be used builder instead")]
    public void SetAddress(IPAddress address)
    {
        var addressAsString = address.ToString();

        _logger.LogInformation($"Setting server address to {addressAsString}...");

        _listener.ListenerAddress = address;

        foreach (var worker in _workerPool!)
            worker.SetHostAddress(addressAsString);

        _logger.LogInformation($"Server address set to {addressAsString} successfully.");
    }

    /// <inheritdoc/>
    [Obsolete("Must be used builder instead")]
    public void SetPort(int port)
    {
        _logger.LogInformation($"Setting server port to {port}...");

        try
        {
            _listener.ListenerPort = port;

            foreach (var worker in _workerPool!)
                worker.SetHostPort(port);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while setting server port to {port}");
            throw;
        }
    }

    /// <summary>
    /// Initializes the internal server components, configuring the listener and preparing worker threads for request handling.
    /// </summary>
    /// <remarks>This method must be called before the server can accept requests. Calling this method
    /// multiple times may result in reinitialization of server components.</remarks>
    /// <param name="logger">The logger instance used to record informational and diagnostic messages during initialization. Cannot be null.</param>
    /// <param name="port">The network port number on which the server will listen for incoming requests. Must be in the valid range for
    /// TCP/UDP ports.</param>
    /// <param name="address">The IP address to which the server listener will bind. Cannot be null.</param>
    private void Initialize(ILogger logger, int port, IPAddress address)
    {
        _logger.LogInformation($"Initializing InternalServer...");

        _listener.ListenerPort = port;
        _listener.ListenerAddress = address;
        _listener.SubscribeToRequestReceived(_eventBus.PublishAsync);
        InitializeWorkers(logger);

        _logger.LogInformation($"InternalServer initialized successfully.");
    }

    /// <summary>
    /// Initializes the server worker pool using the specified logger for diagnostic output.
    /// </summary>
    /// <param name="logger">The logger instance used to record informational messages during worker initialization. Cannot be null.</param>
    private void InitializeWorkers(ILogger logger)
    {
        _logger.LogInformation($"Initializing server workers...");

        _workerPool ??= new ServerWorker[1];

        for (var i = 0; i < _workerPool.Length; i++)
            _workerPool[i] = new(_endpointProviderConfiguration.EndpointContext, _listener.ListenerAddress.ToString(), _listener.ListenerPort, logger);

        _logger.LogInformation($"Server workers initialized successfully.");
    }
}