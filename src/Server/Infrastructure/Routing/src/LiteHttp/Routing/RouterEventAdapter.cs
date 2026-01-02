namespace LiteHttp.Routing;

#nullable disable
public sealed class RouterEventAdapter
{
    private readonly Router _router;

    public RouterEventAdapter() => _router = new();

    public RouterEventAdapter(Router router) => _router = router;

    public void Handle(object? sender, HttpContext httpContext)
    {
        var result = _router.GetAction(httpContext);

        OnCompleted(result);
    }

    private event EventHandler<Func<IActionResult>> Completed;
    private void OnCompleted(Func<IActionResult> action) => Completed?.Invoke(this, action);

    public void SubscribeCompleted(EventHandler<Func<IActionResult>> handler) => Completed += handler;
    public void UnsubscribeCompleted(EventHandler<Func<IActionResult>> handler) => Completed -= handler;
}
