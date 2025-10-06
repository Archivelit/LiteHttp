using System.Net;

namespace AppHost;

public class HttpListener(
    ILogger<HttpListener> logger
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 8080));
        socket.Listen();
        
        #pragma warning disable CA2014
        while (!stoppingToken.IsCancellationRequested)
        {
            var connection = await socket.AcceptAsync(stoppingToken);

            Span<byte> buffer = stackalloc byte[4096];

            int received = connection.Receive(buffer, SocketFlags.None);

            if (received > 0)
            {
                var requestMessage = Encoding.UTF8.GetString(buffer.Slice(0, received));
                logger.LogInformation(requestMessage);
            }

            connection.Close();
        }
        #pragma warning restore
    }
}
