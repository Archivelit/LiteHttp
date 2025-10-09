namespace AppHost;

using LiteHttp.HttpListener;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new HttpListener();
        await listener.StartListen(stoppingToken);
    }
}