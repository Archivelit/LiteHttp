namespace AppHost.HostServices;

public class ServerWorker(
    IRequestSerializer processor,
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var @event = await eventBus.ConsumeAsync(ct);
            var contextString = await processor.DeserializeFromConnectionAsync(@event.Connection, ct);
        }
    }
}