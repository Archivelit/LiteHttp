namespace LiteHttp.HttpListener;

#pragma warning disable CS8618, CA2014
public sealed partial class HttpListener : IHttpListener, IDisposable
{
    public Socket Socket { get => _socket; }
    public int ListenerPort { get => _serverPort; }
    public IPAddress ListenerAddress { get => _ipv4Address; }

    private Socket _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private int _serverPort;
    private IPAddress _ipv4Address;
    private IPEndPoint _endPoint;
    private ListenerState _listenerState = ListenerState.Stopped;

    public HttpListener() =>
        Initialize();

    public HttpListener(int port)
        : this(AddressConstants.IPV4_LOOPBACK, port) { }

    public HttpListener(IPAddress address) 
        : this(address, AddressConstants.DEFAULT_SERVER_PORT) { }

    public HttpListener(IPEndPoint endPoint) =>
        Initialize(endPoint.Address, endPoint.Port);

    public HttpListener(IPAddress address, int port) =>
        Initialize(address, port);

    public async Task StartListen(CancellationToken stoppingToken)
    {
        if (_endPoint is null)
            throw new ArgumentNullException("Listener socket unbound");

        if (!_socket.IsBound)
            BindSocket();
        
        _socket.Listen();
        _listenerState = ListenerState.Listening;

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var connection = await _socket.AcceptAsync(stoppingToken);

                ProcessRequest(connection);
            }
        }
        catch (OperationCanceledException)
        {
            Log.Logger.Debug("Stopping listening");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "An error occurred while listening for incoming connections");
            throw;
        }

        _listenerState = ListenerState.Stopped;
    }

    public void Dispose() =>
        _socket.Dispose();
    
    public HttpListener SetIpAddress(IPAddress address)
    {
        if (IsListening())
            throw new InvalidOperationException("Ip address cannot be changed while server listening");

        _ipv4Address = address;
        UpdateListenerEndPoint();

        return this;
    }
    
    public HttpListener SetPort(int port)
    {
        if (IsListening())
            throw new InvalidOperationException("Port cannot be changed while server listening");

        _serverPort = port;
        UpdateListenerEndPoint();

        return this;
    }

    [SkipLocalsInit]
    private void ProcessRequest(Socket connection)
    {
        Span<byte> buffer = stackalloc byte[4096];

        int received = connection.Receive(buffer, SocketFlags.None);
        
        if (received > 0)
        {
            var request = Encoding.UTF8.GetString(buffer.Slice(0, received));

            Task.Run(() => RaiseRequestReceived(request));
        }
    }

    private void BindSocket() => 
        _socket.Bind(_endPoint);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(_ipv4Address, _serverPort);

    private void Initialize() =>
        Initialize(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT);
    
    private void Initialize(IPAddress iPAddress, int port)
    {
        if (IsListening())
            return;

        _ipv4Address = iPAddress;
        _serverPort = port;

        UpdateListenerEndPoint();
    }
    
    private bool IsListening() => _listenerState == ListenerState.Listening;
}