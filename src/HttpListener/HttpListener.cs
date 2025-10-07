namespace HttpListener;

public class HttpListener : IHttpListener
{
    public Socket Socket { get => _socket; }
    
    private readonly ILogger<HttpListener> _logger;
    private Socket _socket;

    public HttpListener(ILogger<HttpListener> logger)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _logger = logger;    
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        _socket.Bind(new IPEndPoint(new IPAddress([192, 168, 1, 102]), 30000));
        _socket.Listen();

#pragma warning disable CA2014
        while (!stoppingToken.IsCancellationRequested)
        {
            Socket? connection = null;
            try
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
            finally
            {
                if (connection is not null)
                    connection.Close();
                _socket.Close();
            }
        }
#pragma warning restore
    }
}