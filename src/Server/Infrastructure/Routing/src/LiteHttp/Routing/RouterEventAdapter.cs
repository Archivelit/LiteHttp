namespace LiteHttp.Routing;

#nullable disable
public sealed class RouterEventAdapter
{
    private readonly IRouter _router;

    public RouterEventAdapter(IRouter router) => _router = router;

    public void Handle(ConnectionContext connectionContext)
    {
        var action = _router.GetAction(connectionContext.HttpContext);

        if (action is null) OnRequestNotFound(connectionContext);
        
        OnCompleted(connectionContext, action);
    }

    private event Action<ConnectionContext, Func<IActionResult>> Completed;
    private void OnCompleted(ConnectionContext context, Func<IActionResult> action) => Completed?.Invoke(context, action);

    public void SubscribeToCompleted(Action<ConnectionContext, Func<IActionResult>> handler) => Completed += handler;
    public void UnsubscribeFromCompleted(Action<ConnectionContext, Func<IActionResult>> handler) => Completed -= handler;

    private event Action<ConnectionContext, IActionResult> RequestNotFound;
    private void OnRequestNotFound(ConnectionContext context) => RequestNotFound?.Invoke(context, InternalActionResults.NotFound());

    public void SubscribeToRequestNotFound(Action<ConnectionContext, IActionResult> handler) => RequestNotFound += handler;
    public void UnsubscribeFromRequestNotFound(Action<ConnectionContext, IActionResult> handler) => RequestNotFound -= handler;
}
