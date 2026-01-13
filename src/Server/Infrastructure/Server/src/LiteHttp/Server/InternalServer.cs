using System.Runtime.InteropServices;

using LiteHttp.Constants;
using LiteHttp.Heartbeat;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;
using LiteHttp.Models;
using LiteHttp.Pipeline;
using LiteHttp.RequestProcessors;
using LiteHttp.Routing;

namespace LiteHttp.Server;

public sealed class InternalServer : IServer
{
    internal readonly Listener.Listener Listener;
    internal readonly ConnectionManager.ConnectionManager ConnectionManager;
    internal readonly Heartbeat.Heartbeat Heartbeat;
    internal readonly PipelineFactory PipelineFactory;
    private readonly EndpointProviderConfiguration _endpointProviderConfiguration;
    private readonly ILogger<InternalServer> _logger;

    public InternalServer(ILogger? logger, IPAddress address, int port)
    {
        logger ??= NullLogger.Instance;
        _logger = logger.ForContext<InternalServer>();

        var heartbeatHandlers = new List<IHeartbeatHandler>();

        Listener = new(address, port, logger.ForContext<Listener.Listener>());
        ConnectionManager = new();

        heartbeatHandlers.Add(ConnectionManager);
        
        _endpointProviderConfiguration = new EndpointProviderConfiguration();
        
        PipelineFactory = new PipelineFactory(factory =>
        {
            factory.ParserFactory = () => Parser.Instance;
            factory.RouterFactory = () => RouterFactory.Build(_endpointProviderConfiguration.EndpointContext);
            factory.ResponseBuilderFactory = () => new();
            factory.ExecutorFactory = () => new();
        });

        Heartbeat = new (CollectionsMarshal.AsSpan(heartbeatHandlers), 
            _logger.ForContext<Heartbeat.Heartbeat>());
        
        Binder.Bind(this);
    }

    public void Dispose()
    {
        Listener.Dispose();
        ConnectionManager.Dispose();
    }

    public void MapDelete(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Delete, action);
    public void MapGet(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Get, action);
    public void MapPatch(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Patch, action);
    public void MapPost(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Post, action);
    public void MapPut(string route, Func<IActionResult> action) =>
        _endpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Put, action);
    public async Task Start(CancellationToken cancellationToken = default)
    {
        _endpointProviderConfiguration.Freeze();
        
        try
        {
            if (!Listener.StartListen(cancellationToken))
                throw new InvalidOperationException(); // TODO: Add exception string

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception occured");
            throw;
        }
    }
}
