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
        router.RegisterAction("/", "GET", Example.Foo);
        
        while (!ct.IsCancellationRequested)
        {
            var @event = await eventBus.ConsumeAsync(ct);

            var contextString = await serializer.DeserializeFromConnectionAsync(@event.Connection, ct);
            var context = parser.Parse(contextString);
            
            var action = router.GetAction(context.Path, context.Method);
            
            if (action is null)
            {
                // TODO: rework behavior to create response with 404 status code
                Log.Logger.Debug("Unrecognized action");
                continue;
            }

            var actionResult = action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
            else
                response = responseGenerator.Generate(actionResult, "HTTP/1.0");

            await responder.SendResponse(@event.Connection, response);

            @event.Connection.Close();
            @event.Connection.Dispose();
        }
    }
}