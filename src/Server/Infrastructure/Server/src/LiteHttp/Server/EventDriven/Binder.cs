namespace LiteHttp.Server.EventDriven;

internal static class Binder
{
    public static void Bind(InternalServer server)
    {
        server.Listener.SubscribeToRequestReceived(server.ConnectionManager.ReceiveFrom);
        server.ConnectionManager.SubscribeToDataReceived(server.ParserAdapter.ParseRequest);
        server.ParserAdapter.SubscribeToParsed(server.RouterAdapter.GetAction);
        server.RouterAdapter.SubscribeToCompleted(server.ExecutorAdapter.Execute);
        server.ExecutorAdapter.SubscribeToExecuted(server.ResponseBuilderAdapter.BuildResponse);
        server.ResponseBuilderAdapter.SubscriveResponseBuilded(server.ConnectionManager.SendResponse);
        
        server.ParserAdapter.SubscribeToParsingError(server.ResponseBuilderAdapter.BuildResponse);
        
        server.RouterAdapter.SubscribeToRequestNotFound(server.ResponseBuilderAdapter.BuildResponse);
    }
}
