namespace AppHost;

using LiteHttp.Listener;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new Listener();
        await listener.StartListen(stoppingToken);
    }
}