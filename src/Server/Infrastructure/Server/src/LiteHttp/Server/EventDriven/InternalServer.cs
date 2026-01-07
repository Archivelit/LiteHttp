using LiteHttp.Constants;
using LiteHttp.Listener;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;
using LiteHttp.Models;
using LiteHttp.RequestProcessors.Adapters;
using LiteHttp.Routing;

namespace LiteHttp.Server.EventDriven;

public sealed class InternalServer : IServer
{
    internal readonly ParserEventAdapter ParserAdapter;
    internal readonly RouterEventAdapter RouterAdapter;
    internal readonly ResponseBuilderEventAdapter ResponseBuilderAdapter;
    internal readonly SaeaListener Listener;
    internal readonly ConnectionManager.ConnectionManager ConnectionManager;
    internal readonly ExecutorEventAdapter ExecutorAdapter;
    internal readonly Heartbeat.Heartbeat Heartbeat;
    private readonly IEndpointProviderConfiguration EndpointProviderConfiguration;
    private readonly ILogger<InternalServer> _logger;

    public InternalServer(ILogger? logger, IPAddress address, int port)
    {
        logger ??= NullLogger.Instance;
        _logger = logger.ForContext<InternalServer>();

        Listener = new(address, port, logger.ForContext<SaeaListener>());
        ParserAdapter = new();
        ResponseBuilderAdapter = new();
        ConnectionManager = new();
        ExecutorAdapter = new();

        EndpointProviderConfiguration = new EndpointProviderConfiguration();

        var router = RouterFactory.Build(EndpointProviderConfiguration.EndpointContext);

        RouterAdapter = new(router);

        Heartbeat = new Heartbeat.Heartbeat([ConnectionManager], _logger.ForContext<Heartbeat.Heartbeat>());
        
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
