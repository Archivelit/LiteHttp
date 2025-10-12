namespace AppHost.HostServices;

public class ServerWorker(
    IRequestSerializer processor,
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var @event = await eventBus.ConsumeAsync(ct);
        await processor.DeserializeFromConnectionAsync(@event.Connection ,ct);
    }
}