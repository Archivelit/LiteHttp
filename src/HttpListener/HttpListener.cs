namespace LiteHttp.HttpListener;

#pragma warning disable CS8618, CA2014
public class HttpListener : IHttpListener, IDisposable
{
    public Socket Socket { get => _socket; }
    public int ListenerPort { get => _serverPort; }

    private int _serverPort;
    private IPAddress _IPV4Address;
    private IPEndPoint _endPoint;
    private Socket _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
        {
            throw new ArgumentNullException("Listener socket unbound");
        }

        if (!_socket.IsBound)
        {
            BindSocket();
        }

        _socket.Listen();

        _listenerState = ListenerState.Listening;

        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = await _socket.AcceptAsync(stoppingToken);
            
            Span<byte> buffer = stackalloc byte[4096];

            int received = connection.Receive(buffer, SocketFlags.None);

            if (received > 0)
            {
                var requestMessage = Encoding.UTF8.GetString(buffer.Slice(0, received));
            }
        }

        _listenerState = ListenerState.Stopped;
    }

    public void Dispose() =>
        _socket.Dispose();

    private void BindSocket() => 
        _socket.Bind(_endPoint);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(_IPV4Address, _serverPort);

    private void Initialize() =>
        Initialize(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT);

    private void Initialize(IPAddress iPAddress, int port)
    {
        if (_listenerState == ListenerState.Listening)
        {
            return;
        }

        _IPV4Address = iPAddress;
        _serverPort = port;

        UpdateListenerEndPoint();
    }
}