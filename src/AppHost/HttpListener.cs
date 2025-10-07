namespace AppHost;

using HttpListener;

public class Worker(
    IHttpListener listener
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await listener.ListenAsync(stoppingToken);
        }    
    }
}
