namespace LiteHttp.Server.EventDriven;

internal static class Binder
{
    public static void Bind(InternalServer server)
    {
        // server.Listener.SubscribeToRequestAccepted(server.ConnectionManager.Process);
        // server.ConnectionManager.SubscribeToConnectionReceived(server.ParserAdapter.Handle);
        server.ParserAdapter.SubscribeParsed(server.RouterAdapter.Handle);
        // server.RouterAdapter.SubscribeToCompleted(server.ExecutorAdapter.Handle);
        // server.ExecutorAdapter.SubscribeToExecuted(server.ResponseBuilderAdapter.Handle);
        // server.ResponseBuilderAdapter.SubscriveResponseBuilded(server.ConnectionManager.SendResponse);
    }
}
