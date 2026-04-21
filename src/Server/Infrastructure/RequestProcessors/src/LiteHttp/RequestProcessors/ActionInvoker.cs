namespace LiteHttp.RequestProcessors;

public sealed class ActionInvoker
{
    public IActionResult Execute(Func<IActionResult> action) => action();
}
