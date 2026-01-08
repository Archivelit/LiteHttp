using System.Runtime.InteropServices;

using LiteHttp.Constants;
using LiteHttp.Heartbeat;
using LiteHttp.Listener;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;
using LiteHttp.Models;
using LiteHttp.Pipeline;
using LiteHttp.RequestProcessors;
using LiteHttp.Routing;

namespace LiteHttp.Server.EventDriven;

public sealed class InternalServer : IServer
{
    internal readonly SaeaListener Listener;
    internal readonly ConnectionManager.ConnectionManager ConnectionManager;
    internal readonly Heartbeat.Heartbeat Heartbeat;
    internal readonly PipelineFactory PipelineFactory;
    private readonly IEndpointProviderConfiguration EndpointProviderConfiguration;
    private readonly ILogger<InternalServer> _logger;

    public InternalServer(ILogger? logger, IPAddress address, int port)
    {
        logger ??= NullLogger.Instance;
        _logger = logger.ForContext<InternalServer>();

        var heartbeatHandlers = new List<IHeartbeatHandler>();
        
        Listener = new(address, port, logger.ForContext<SaeaListener>());
        ConnectionManager = new();

        heartbeatHandlers.Add(ConnectionManager);
        
        EndpointProviderConfiguration = new EndpointProviderConfiguration();
        
        PipelineFactory = new PipelineFactory(factory =>
        {
            factory.ParserFactory = () => Parser.Instance;
            factory.RouterFactory = () => RouterFactory.Build(EndpointProviderConfiguration.EndpointContext);
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
        EndpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Delete, action);
    public void MapGet(string route, Func<IActionResult> action) =>
        EndpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Get, action);
    public void MapPatch(string route, Func<IActionResult> action) =>
        EndpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Patch, action);
    public void MapPost(string route, Func<IActionResult> action) =>
        EndpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Post, action);
    public void MapPut(string route, Func<IActionResult> action) =>
        EndpointProviderConfiguration.AddEndpoint(route.AsMemoryByteArray(), RequestMethodsAsBytes.Put, action);

    [Obsolete("Builder must be used")]
    public void SetAddress(string address) => throw new NotImplementedException("Use builder pattern");
    [Obsolete("Builder must be used")]
    public void SetPort(int port) => throw new NotImplementedException("Use builder pattern");

    public async Task Start(CancellationToken cancellationToken = default)
    {
        EndpointProviderConfiguration.Freeze();
        
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
