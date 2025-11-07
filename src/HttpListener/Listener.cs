
namespace LiteHttp.Listener;

#pragma warning disable CS8618
public sealed partial class Listener : IListener, IDisposable
{
    public Socket Socket { get => _socket; }
    public int ListenerPort { get => _serverPort; }
    public IPAddress ListenerAddress { get => _ipv4Address; }
    
    private Socket _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private int _serverPort;
    private IPAddress _ipv4Address;
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

        if (!_socket.IsBound)
        {
            BindSocket();
            _logger.LogDebug($"Socket bound");
        }

        _socket.Listen();

        _listenerState = ListenerState.Listening;

        _logger.LogInformation($"Listening at {_endPoint.ToString()}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var connection = await _socket.AcceptAsync(stoppingToken).ConfigureAwait(false);

                _logger.LogDebug($"Request accepted");

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

    public void Dispose()
    {
        _socket.Dispose();
    }

    public Listener SetIpAddress(IPAddress address)
    {
        if (IsListening())
            throw new InvalidOperationException("Ip address cannot be changed while server listening");

        _ipv4Address = address;
        UpdateListenerEndPoint();

        return this;
    }
    
    public Listener SetPort(int port)
    {
        if (IsListening())
            throw new InvalidOperationException("Port cannot be changed while server listening");

        _serverPort = port;
        UpdateListenerEndPoint();

        return this;
    }

    private void BindSocket() => 
        _socket.Bind(_endPoint);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(_ipv4Address, _serverPort);

    private void Initialize() =>
        Initialize(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT);
    
    private void Initialize(IPAddress iPAddress, int port)
    {
        _ipv4Address = iPAddress;
        _serverPort = port;

        UpdateListenerEndPoint();
    }
    
    private bool IsListening() => _listenerState == ListenerState.Listening;
}