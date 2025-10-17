using System.Net.Sockets;
using LiteHttp.Constants;

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
            var context = parser.Parse(contextString);
            
            var action = router.GetAction(context.Path, context.Method);
            
            if (action is null)
            {
                Log.Logger.Debug("Unrecognized action");
                
                var notFoundResponse = responseGenerator.Generate(new ActionResult(ResponseCode.NotFound), "HTTP/1.0");
                await SendResponseAndDisposeConnection(@event.Connection, notFoundResponse);
                
                continue;
            }

            var actionResult = action();

            string response;

            if (actionResult is IActionResult<object> result)
                response = responseGenerator.Generate(result, "HTTP/1.0", result.ToString());
            else
                response = responseGenerator.Generate(actionResult, "HTTP/1.0");

            await SendResponseAndDisposeConnection(@event.Connection, response);
        }

        async Task SendResponseAndDisposeConnection(Socket connection, string response)
        {
            await responder.SendResponse(connection, response);
            
            connection.Close();
            connection.Dispose();            
        }
    }
}