namespace LiteHttp.Server;

/// <summary>
/// Provides an HTTP server implementation that allows mapping routes to request handlers.
/// </summary>
/// <remarks>Use this class to host an HTTP server and define handlers
/// for HTTP methods such as GET, POST, PUT, PATCH, and DELETE. The server supports asynchronous startup and can be
/// integrated with cancellation tokens for graceful shutdown. This class is sealed and cannot be inherited. It
/// implements <see cref="IServer"/> and <see cref="IDisposable"/> for server lifecycle management.</remarks>
public sealed class HttpServer : IServer, IDisposable
{
    private readonly InternalServer _internalServer;
    
    /// <summary>
    /// Initializes a new instance of the HttpServer class with the specified <see cref="InternalServer"/>.
    /// </summary>
    /// <param name="internalServer">Preconfigured <see cref="InternalServer"/> instance.</param>
    internal HttpServer(InternalServer internalServer) => _internalServer = internalServer;

    /// <inheritdoc/>
    public Task Start(CancellationToken cancellationToken = default) =>
        _internalServer.Start(cancellationToken);

    /// <inheritdoc/>
    [Obsolete("Use builder instead")]
    public void SetPort(int port) =>
        _internalServer.SetPort(port);

    /// <inheritdoc/>
    [Obsolete("Use builder instead")]
    public void SetAddress(string address) =>
        _internalServer.SetAddress(address);

    /// <inheritdoc/>
    public void MapGet(string route, Func<IActionResult> action) =>
        _internalServer.MapGet(route, action);

    /// <inheritdoc/>
    public void MapDelete(string route, Func<IActionResult> action) =>
        _internalServer.MapDelete(route, action);

    /// <inheritdoc/>
    public void MapPost(string route, Func<IActionResult> action) =>
        _internalServer.MapPost(route, action);

    /// <inheritdoc/>
    public void MapPut(string route, Func<IActionResult> action) =>
        _internalServer.MapPut(route, action);

    /// <inheritdoc/>
    public void MapPatch(string route, Func<IActionResult> action) =>
        _internalServer.MapPatch(route, action);

    /// <summary>
    /// Releases all resources used by the current instance.
    /// </summary>
    /// <remarks>Call this method when you are finished using the instance to free unmanaged resources and
    /// perform necessary cleanup. After calling <see cref="Dispose"/>, the instance should not be used.</remarks>
    public void Dispose() =>
        _internalServer.Dispose();
}