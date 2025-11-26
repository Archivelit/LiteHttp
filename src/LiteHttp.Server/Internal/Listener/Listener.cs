namespace LiteHttp.Listener;

#pragma warning disable CS8618
internal sealed partial class Listener : IListener, IDisposable
{
    private Socket Socket { get; } = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public int ListenerPort { get; private set; }
    public IPAddress ListenerAddress { get; private set; }

    private IPEndPoint _endPoint;
    private ListenerState _listenerState = ListenerState.Stopped;
    private ILogger<Listener> _logger = NullLogger<Listener>.Instance;

    public Listener(ILogger<Listener>? logger = null)
    {
        if (logger is not null)
            _logger = logger;

        Initialize();
    }

    public Listener(int port)
        : this(AddressConstants.IPV4_LOOPBACK, port) { }

    public Listener(IPAddress address)
        : this(address, AddressConstants.DEFAULT_SERVER_PORT) { }

    public Listener(IPEndPoint endPoint, ILogger<Listener>? logger = null)
    {
        if (logger is not null)
            _logger = logger;

        Initialize(endPoint.Address, endPoint.Port);
    }

    public Listener(IPAddress address, int port, ILogger<Listener>? logger = null)
    {
        if (logger is not null)
            _logger = logger;

        Initialize(address, port);
    }

    public async ValueTask StartListen(CancellationToken stoppingToken)
    {
        if (_endPoint is null)
            throw new ArgumentNullException(nameof(_endPoint), "Listener endpoint cannot be null");

        if (!Socket.IsBound)
        {
            BindSocket();
            _logger.LogDebug($"Socket bound");
        }

        Socket.Listen();

        _listenerState = ListenerState.Listening;

        _logger.LogInformation($"Listening at {_endPoint.ToString()}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var connection = await Socket.AcceptAsync(stoppingToken).ConfigureAwait(false);

                _logger.LogInformation($"Request accepted");

                OnRequestReceived(new RequestReceivedEvent(connection), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while listening for incoming connections");
            throw;
        }
        finally
        {
            _listenerState = ListenerState.Stopped;
        }
    }

    public void Dispose() => 
        Socket.Dispose();

    internal Listener SetIpAddress(IPAddress address)
    {
        if (IsListening())
            throw new InvalidOperationException("Ip address cannot be changed while server listening");

        ListenerAddress = address;
        UpdateListenerEndPoint();

        return this;
    }

    internal Listener SetPort(int port)
    {
        if (IsListening())
            throw new InvalidOperationException("Port cannot be changed while server listening");

        ListenerPort = port;
        UpdateListenerEndPoint();

        return this;
    }

    internal Listener SetLogger(ILogger logger)
    {
        _logger = logger.ForContext<Listener>();

        return this;
    }

    private void BindSocket() =>
        Socket.Bind(_endPoint);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(ListenerAddress, ListenerPort);

    private void Initialize() =>
        Initialize(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT);

    private void Initialize(IPAddress iPAddress, int port)
    {
        ListenerAddress = iPAddress;
        ListenerPort = port;

        UpdateListenerEndPoint();
    }

    private bool IsListening() => _listenerState == ListenerState.Listening;
}