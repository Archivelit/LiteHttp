namespace AppHost.HostServices;

#nullable disable
public class ServerWorker(
    IRequestSerializer serializer,
    IRequestParser parser,
    IRouter router,
    IResponseGenerator responseGenerator,
    IResponder responder,
    IEventBus<RequestReceivedEvent> eventBus
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var @event = await eventBus.ConsumeAsync(ct);

            var contextString = await serializer.DeserializeFromConnectionAsync(@event.Connection, ct);

            Log.Logger.Information(contextString);
            
            var context = parser.Parse(contextString);
            
            var action = router.GetAction(context.Path, context.Method);
            
            if (action is null)
            {
                // TODO: rework behavior to create response with 404 status code
                Log.Logger.Debug("Unrecognized action");
                continue;
            }

            var actionResult = await action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = responseGenerator.Generate(actionResult, "HTTP\\1.1", result.Result.ToString());
            else
                response = responseGenerator.Generate(actionResult, "HTTP\\1.1");

            await responder.SendResponse(@event.Connection, response);
        }
    }
}