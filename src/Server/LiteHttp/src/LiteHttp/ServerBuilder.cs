using EventDrivenInternalServer = LiteHttp.Server.EventDriven.InternalServer;

namespace LiteHttp;

/// <summary>
/// Provides a builder with fluent api for configuring and creating instances of <see cref="HttpServer"/> 
/// with customizable settings such as logger, worker count etc. The builder has no logging inside
/// </summary>
/// <remarks>
/// Thread safety is not guaranteed; configure the builder on a single
/// thread before building the server.
/// </remarks>
public class ServerBuilder
{
    private ILogger _logger = NullLogger.Instance;
    private int _workersCount = Environment.ProcessorCount / 2;
    private int _port = AddressConstants.DEFAULT_SERVER_PORT;
    private IPAddress _address = AddressConstants.IPV4_LOOPBACK;

    /// <summary>
    /// Creates and configures a new instance of the <see cref="HttpServer"/> class using the specified worker count,
    /// port, etc.
    /// </summary>
    /// <remarks>
    /// The returned server instance is initialized with the parameters provided to the builder.
    /// Ensure that the configuration properties are set appropriately before calling this method. This method does not
    /// start the server; you must explicitly start the returned.
    /// <see cref="HttpServer"/> instance.</remarks>
    /// <returns>A configured <see cref="HttpServer"/> instance.</returns>
    public HttpServer Build()
    {
        var internalServer = new InternalServer(workersCount: _workersCount, port: _port, address: _address, 
            logger: _logger);

        return new(internalServer);
    }

    public EventDrivenInternalServer BuildEventDrivenServer() => new(logger: _logger, address: _address, port: _port);

    /// <summary>
    /// Configures the server builder to use the specified logger for diagnostic and operational messages.
    ///  <see cref="NullLogger"/> is predefined logger for the server, use this method 
    ///  only if you want to use custom <see cref="ILogger"/> implementation such as logging framework adapters etc.
    ///  <para>Changes will not reflect on building stage cause <see cref="ServerBuilder"/> does not provide logging</para>
    /// </summary>
    /// <remarks>
    /// Calling this method replaces any previously configured logger.
    /// </remarks>
    /// <param name="logger">The logger instance to use for logging server events. Cannot be null. </param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with the configured logger.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null</exception>
    public ServerBuilder WithLogger(ILogger logger)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger), $"Logger cannot be null");

        return this;
    }

    /// <summary>
    /// Configures the number of worker threads to be used by the server.
    /// </summary>
    /// <param name="workersCount">The number of worker threads to allocate. Must be greater than or equal to 1.</param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with the updated worker count.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="workersCount"/> is less than 1.</exception>
    public ServerBuilder WithWorkersCount(int workersCount)
    {
        if (workersCount < 1)
            throw new ArgumentException("Workers count cannot be under 1 worker");
        _workersCount = workersCount;

        return this;
    }

    /// <summary>
    /// Sets the port number for the server to listen on.
    /// </summary>
    /// <param name="port">The port number to assign to the server. Must be zero or greater.</param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with the specified port configured.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="port"/> is less than zero.</exception>
    public ServerBuilder WithPort(int port)
    {
        if (port < 0)
            throw new ArgumentException("Port cannot be below zero");

        _port = port;

        return this;
    }

    /// <summary>
    /// Sets the server's network address to the specified value.
    /// </summary>
    /// <param name="address">The IP address to assign to the server. Cannot be null.</param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with the updated address.</returns>
    public ServerBuilder WithAddress(IPAddress address)
    {
        _address = address;

        return this;
    }

    /// <summary>
    /// Sets the server's network address using the specified host name or IP address.
    /// </summary>
    /// <remarks>
    /// Only IPv4 addresses are considered. If the host cannot be resolved or contains no IPv4 entries,
    /// an exception will be thrown.
    /// <para>
    /// If multiple addresses are resolved for the specified host, the first IPv4 address is used.
    /// </para>
    /// </remarks>
    /// <param name="address">The host name or IPv4 address to use for the server. An <see cref="ArgumentNullException"/> is thrown if null.</param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with the updated address.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="address"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="address"/> length is greater than 255 characters.</exception>
    /// <exception cref="SocketException">An error is encountered when resolving.<paramref name="address"/></exception>
    /// <exception cref="ArgumentException"><paramref name="address"/> is invalid IP address.</exception>
    public ServerBuilder WithAddress(string address)
    {
        _address = Dns.GetHostAddresses(address)
            .First(a => a.AddressFamily == AddressFamily.InterNetwork);

        return this;
    }
    
    /*
    /// <summary>
    /// Configures server resource limits using the specified settings.
    /// </summary>
    /// <remarks>
    /// Subsequent calls will overwrite previous limit settings.
    /// </remarks>
    /// <param name="configuration">The limits configuration to apply to the server. Cannot be null.</param>
    /// <returns>The current <see cref="ServerBuilder"/> instance with updated limits configuration.</returns>
    public ServerBuilder WithLimits(LimitsConfiguration configuration)
    {
        _limitsProvider = new LimitsProvider(configuration);

        return this;
    }
    */
}