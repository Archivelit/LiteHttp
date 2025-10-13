namespace AppHost.HostServices;

public class ServerWorker(
    IRequestSerializer serializer,
    IRequestParser requestParser,
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var @event = await eventBus.ConsumeAsync(ct);
            
            var contextString = await serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
            var context = requestParser.Parse(contextString);
        }
    }
}