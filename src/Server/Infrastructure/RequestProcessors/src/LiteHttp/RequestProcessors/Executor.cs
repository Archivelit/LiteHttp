namespace LiteHttp.RequestProcessors;

public sealed class Executor
{
    public IActionResult Execute(Func<IActionResult> action)
    {
        return action();
    }
}
