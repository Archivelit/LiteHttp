using LiteHttp.Server;

namespace LiteHttp.Abstractions;

public interface IServer
{
    /// <summary>
    /// Starts the server asynchronously, allowing cancellation via the provided token.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the start operation before it completes. If not specified,
    /// the operation cannot be cancelled.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public Task Start(CancellationToken cancellationToken);

    /// <summary>
    /// Sets the port number for the internal server. This method is obsolete; use <see cref="ServerBuilder.WithPort"/> instead
    /// </summary>
    /// <remarks>Calling this method is deprecated. For new code, use the recommended builder approach to
    /// configure the server address</remarks>
    /// <param name="port">The port number to assign to the internal server. Must be within the valid range for TCP/UDP ports (0–65535).</param>
    [Obsolete("Use builder instead")]
    public void SetPort(int port);

    /// <summary>
    /// Sets the server address for the current instance. This method is obsolete; use <see cref="ServerBuilder.WithAddress"/> instead
    /// </summary>
    /// <remarks>This method is marked as obsolete. For new code, use the recommended builder approach to
    /// configure the server address.</remarks>
    /// <param name="address">The address to assign to the server. Cannot be null or empty.</param>
    [Obsolete("Use builder instead")]
    public void SetAddress(string address);

    /// <summary>
    /// Maps an HTTP GET request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for GET endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for GET requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapGet(string route, Func<IActionResult> action);

    /// <summary>
    /// Maps an HTTP DELETE request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for DELETE endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for DELETE requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapDelete(string route, Func<IActionResult> action);

    /// <summary>
    /// Maps an HTTP POST request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for POST endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for POST requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapPost(string route, Func<IActionResult> action);

    /// <summary>
    /// Maps an HTTP PUT request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for PUT endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for PUT requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapPut(string route, Func<IActionResult> action);

    /// <summary>
    /// Maps an HTTP PATCH request to the specified route and associates it with the provided action to handle incoming
    /// requests.
    /// </summary>
    /// <remarks>Use this method to register handlers for PATCH endpoints in your application. The action will
    /// be invoked whenever a request matches the specified route.</remarks>
    /// <param name="route">The route template that defines the URL pattern to match for PATCH requests. Must not be null or empty.</param>
    /// <param name="action">A delegate that processes the request and returns an <see cref="IActionResult"/> representing the response.
    /// Cannot be null.</param>
    public void MapPatch(string route, Func<IActionResult> action);
}