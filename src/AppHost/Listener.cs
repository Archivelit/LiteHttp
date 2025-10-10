namespace AppHost;

public class Listener(
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new LiteHttp.Listener.Listener();
        listener.SubscribeToRequestReceived(eventBus.Publish);
        await listener.StartListen(stoppingToken);
    }
}