using LiteHttp.RequestProcessors;

namespace LiteHttp.Server.EventDriven;

internal static class Binder
{
    public static void Bind(InternalServer server)
    {
        server.Listener.SubscribeToRequestReceived(server.ConnectionManager.ReceiveFrom);
        server.ConnectionManager.SubscribeToDataReceived(server.ParserAdapter.Handle);
        server.ParserAdapter.SubscribeToParsed(server.RouterAdapter.Handle);
        server.RouterAdapter.SubscribeToCompleted(server.ExecutorAdapter.Handle);
        server.ExecutorAdapter.SubscribeToExecuted(server.ResponseBuilderAdapter.Handle);
        server.ResponseBuilderAdapter.SubscriveResponseBuilded(server.ConnectionManager.SendResponse);
        
        server.ParserAdapter.SubscribeToParsingError(server.ResponseBuilderAdapter.Handle);
        
        server.RouterAdapter.SubscribeToRequestNotFound(server.ResponseBuilderAdapter.Handle);
    }
}
