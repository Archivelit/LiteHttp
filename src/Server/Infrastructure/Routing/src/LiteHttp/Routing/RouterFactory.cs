namespace LiteHttp.Routing;

public sealed class RouterFactory
{
    public static IRouter Build(IEndpointContext context)
    {
        var router = new Router();

        router.SetContext(context);
        
        return router;
    }
}