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
    /// Initializes a new instance of the HttpServer class with the specified configuration
    /// </summary>
    /// <remarks>If no address is provided, the server will only accept connections from the local machine.
    /// Providing a custom logger allows integration with application-wide logging frameworks.</remarks>
    /// <param name="workersCount">The number of worker threads to handle incoming HTTP requests. Must be greater than zero.</param>
    /// <param name="port">The network port on which the server listens for incoming connections. Defaults to
    /// AddressConstants.DEFAULT_SERVER_PORT if not specified.</param>
    /// <param name="address">The IP address to bind the server to. If null, the server binds to the loopback address.</param>
    /// <param name="logger">The logger used for server diagnostics and error reporting. If null, a no-op logger is used.</param>
    internal HttpServer(int workersCount = 1, int port = AddressConstants.DEFAULT_SERVER_PORT, 
        IPAddress? address = null, ILogger? logger = null)
    {
        logger ??= NullLogger.Instance;
        address ??= IPAddress.Loopback;

        _internalServer = new InternalServer(workersCount: workersCount, address: address, port: port, logger: logger);
    }
    
    /// <summary>
    /// Starts the server asynchronously, allowing cancellation via the provided token.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the start operation before it completes. If not specified,
    /// the operation cannot be cancelled.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public Task Start(CancellationToken cancellationToken = default) =>
        _internalServer.Start(cancellationToken);

    /// <summary>
    /// Sets the port number for the internal server. This method is obsolete; use <see cref="ServerBuilder.WithPort"/> instead
    /// </summary>
    /// <remarks>Calling this method is deprecated. For new code, use the recommended builder approach to
    /// configure the server address</remarks>
    /// <param name="port">The port number to assign to the internal server. Must be within the valid range for TCP/UDP ports (0–65535).</param>
    [Obsolete("Use builder instead")]
    public void SetPort(int port) => 
        _internalServer.SetPort(port);

    /// <summary>
    /// Sets the server address for the current instance. This method is obsolete; use <see cref="ServerBuilder.WithAddress"/> instead
    /// </summary>
    /// <remarks>This method is marked as obsolete. For new code, use the recommended builder approach to
    /// configure the server address.</remarks>
    /// <param name="address">The address to assign to the server. Cannot be null or empty.</param>
    [Obsolete("Use builder instead")]
    public void SetAddress(string address) =>
        _internalServer.SetAddress(address);
    
    /// <summary>
    /// Maps an HTTP GET request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for GET endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for GET requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapGet(string route, Func<IActionResult> action) =>
        _internalServer.MapGet(route, action);

    /// <summary>
    /// Maps an HTTP DELETE request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for DELETE endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for DELETE requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapDelete(string route, Func<IActionResult> action) =>
        _internalServer.MapDelete(route, action);

    /// <summary>
    /// Maps an HTTP POST request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for POST endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for POST requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapPost(string route, Func<IActionResult> action) =>
        _internalServer.MapPost(route, action);

    /// <summary>
    /// Maps an HTTP PUT request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for PUT endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for PUT requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapPut(string route, Func<IActionResult> action) =>
        _internalServer.MapPut(route, action);

    /// <summary>
    /// Maps an HTTP PATCH request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for PATCH endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for PATCH requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
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