namespace AppHost;

public class Listener : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new LiteHttp.Listener.Listener();
        await listener.StartListen(stoppingToken);
    }
}