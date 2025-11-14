namespace LiteHttp.Server;

public class ServerBuilder
{
    private ILogger _logger = NullLogger.Instance;
    private int _workersCount = Environment.ProcessorCount / 2;
    private int _port = AddressConstants.DEFAULT_SERVER_PORT;
    private IPAddress _address = AddressConstants.IPV4_LOOPBACK;
    
    public HttpServer Build()
    {
        return new HttpServer(workersCount: _workersCount, port: _port, address: _address, logger: _logger);
    }

    public ServerBuilder WithLogger(ILogger logger)
    {
        _logger = logger;

        return this;
    }

    public ServerBuilder WithWorkersCount(int workersCount)
    {
        if (workersCount < 1)
            throw new ArgumentException("Workers count cannot be under 1 worker");
        _workersCount = workersCount;
        
        return this;
    }

    public ServerBuilder WithPort(int port)
    {
        if (port < 0)
            throw new ArgumentException("Port cannot be below zero");

        _port = port;
        
        return this;
    }

    public ServerBuilder WithAddress(IPAddress address)
    {
        _address = address;

        return this;
    }

    public ServerBuilder WithAddress(string address)
    {
        _address = Dns.GetHostAddresses(address)
            .First(a => a.AddressFamily == AddressFamily.InterNetwork);

        return this;
    }
}