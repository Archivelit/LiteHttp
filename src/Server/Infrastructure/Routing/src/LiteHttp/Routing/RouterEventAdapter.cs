namespace LiteHttp.Routing;

#nullable disable
public sealed class RouterEventAdapter
{
    private readonly IRouter _router;

    public RouterEventAdapter(IRouter router) => _router = router;

    public void Handle(ConnectionContext connectionContext)
    {
        var result = _router.GetAction(connectionContext.HttpContext);

        OnCompleted(connectionContext, result);
    }

    private event Action<ConnectionContext, Func<IActionResult>> Completed;
    private void OnCompleted(ConnectionContext context, Func<IActionResult> action) => Completed?.Invoke(context, action);

    public void SubscribeToCompleted(Action<ConnectionContext, Func<IActionResult>> handler) => Completed += handler;
    public void UnsubscribeCompleted(Action<ConnectionContext, Func<IActionResult>> handler) => Completed -= handler;
}
