namespace HttpListener;

#pragma warning disable CS8618, CA2014
public class HttpListener : IHttpListener, IDisposable
{
    public Socket Socket { get => _socket; }
    public int ListenerPort { get => _serverPort; }

    private int _serverPort;
    private IPAddress _IPV4Address;
    private IPEndPoint _endPoint;
    private Socket _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public HttpListener() =>
        Initialize();

    public HttpListener(IPEndPoint endPoint) =>
        Initialize(endPoint.Address, endPoint.Port);

    public HttpListener(int port) =>
        Initialize(port);

    public HttpListener(IPAddress address) =>
        Initialize(address);

    public HttpListener(IPAddress address, int port) =>
        Initialize(address, port);

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        if (!_socket.IsBound)
        {
            BindSocket();
        }

        _socket.Listen();

        while (!stoppingToken.IsCancellationRequested)
        {
            using(var connection = await _socket.AcceptAsync(stoppingToken))
            {
                Span<byte> buffer = stackalloc byte[4096];

                int received = connection.Receive(buffer, SocketFlags.None);

                if (received > 0)
                {
                    var requestMessage = Encoding.UTF8.GetString(buffer.Slice(0, received));
                }
            }
        }
        Dispose();
    }

    public void Dispose() =>
        _socket.Dispose();

    private void BindSocket() => 
        _socket.Bind(_endPoint);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(_IPV4Address, _serverPort);

    private void Initialize() =>
        Initialize(new([192, 168, 1, 102]), 30000);

    private void Initialize(IPAddress iPAddress) =>
        Initialize(iPAddress, _serverPort);

    private void Initialize(int port) =>
        Initialize(_IPV4Address, port);

    private void Initialize(IPAddress iPAddress, int port)
    {
        OnEndPointUpdate += UpdateListenerEndPoint;

        _IPV4Address = iPAddress;
        _serverPort = port;

        OnEndPointUpdate.Invoke();
    }

    private event Action OnEndPointUpdate;
}