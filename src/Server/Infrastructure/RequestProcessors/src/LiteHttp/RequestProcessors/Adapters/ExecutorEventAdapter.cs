namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ExecutorEventAdapter
{
    private readonly Executor _executor = new();

    public void Execute(ConnectionContext context, Func<IActionResult> action)
    {
        var result = _executor.Execute(action);

        OnExecuted(context, result);
    }

    private event Action<ConnectionContext, IActionResult> Executed;
    private void OnExecuted(ConnectionContext context, IActionResult action) => Executed?.Invoke(context, action);

    public void SubscribeToExecuted(Action<ConnectionContext, IActionResult> action) => Executed += action;
    public void UnsubscribeFromExecuted(Action<ConnectionContext, IActionResult> action) => Executed -= action;
}
