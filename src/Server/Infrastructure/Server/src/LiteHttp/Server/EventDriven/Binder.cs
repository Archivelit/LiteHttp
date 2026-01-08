namespace LiteHttp.Server.EventDriven;

internal static class Binder
{
    public static void Bind(InternalServer server)
    {
        server.Listener.SubscribeToRequestReceived(server.ConnectionManager.ReceiveFrom);
        server.ConnectionManager.SubscribeToDataReceived(ctx =>
        {
            var pipeline = ThreadLocalPipelines.Current;
            if (pipeline is null)
            {
                pipeline = server.PipelineFactory.BuildPipeline();
                ThreadLocalPipelines.Current = pipeline;

                pipeline.SubscribeToExecuted(server.ConnectionManager.SendResponse);
            }

            pipeline.ProcessRequest(ctx);
        });
    }
}
