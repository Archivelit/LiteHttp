namespace AppHost.HostServices;

#nullable disable
public class ServerWorker(
    IRequestSerializer serializer,
    IRequestParser parser,
    IRouter router,
    IResponseGenerator responseGenerator,
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var @event = await eventBus.ConsumeAsync(ct);

            var contextString = await serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
            var context = parser.Parse(contextString);
            
            var action = router.GetAction(context.Path, context.Method);
            
            if (action is null)
            {
                continue;
                // TODO: rework behavior to create response with 404 status code
            }

            var actionResult = await action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = responseGenerator.Generate(actionResult, "HTTP\\1.1", result.Result.ToString());
            else
                responseGenerator.Generate(actionResult, "HTTP\\1.1");


        }
    }
}