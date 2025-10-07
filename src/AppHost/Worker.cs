namespace AppHost;

using HttpListener;

public class Worker(
    IHttpListener listener
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await listener.ListenAsync(stoppingToken);
    }
}