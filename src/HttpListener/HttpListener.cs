namespace HttpListener;

public class HttpListener : IHttpListener, IDisposable
{
    public Socket Socket { get => _socket; }
    public int DefaultPort { get => _defaultPort; }

    private readonly ILogger<HttpListener> _logger;
    private readonly int _defaultPort = 30000;
    
    private Socket _socket;

    public HttpListener(ILogger<HttpListener> logger)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _logger = logger;    
    }

    public HttpListener(ILogger<HttpListener> logger, IPEndPoint endPoint) 
        : this(logger)
    {
        _socket.Bind(endPoint);
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        if (!_socket.IsBound)
        {
            BindDefault();
        }

        _socket.Listen();

        Socket? connection = null;

#pragma warning disable CA2014
        while (!stoppingToken.IsCancellationRequested)
        {
            connection = await _socket.AcceptAsync(stoppingToken);

            Span<byte> buffer = stackalloc byte[4096];

            int received = connection.Receive(buffer, SocketFlags.None);

            if (received > 0)
            {
                var requestMessage = Encoding.UTF8.GetString(buffer.Slice(0, received));
                _logger.LogInformation(requestMessage);
            }
        }
#pragma warning restore

        connection?.Dispose();
        Dispose();
    }

    public void Dispose() =>
        _socket.Dispose();

    private void BindSocket(IPEndPoint endPoint) => 
        _socket.Bind(endPoint);

    private void BindDefault() =>
        BindSocket(new IPEndPoint(new IPAddress([192, 168, 1, 102]), _defaultPort));
}