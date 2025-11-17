namespace LiteHttp.Models;

public readonly record struct Result<TResult>
{
    public Exception? Exception { get; init; }
    public TResult? Value { get; init; }
    public bool Success { get; init; }

    public Result(Exception exception)
    {
        Exception = exception;
        Success = false;
    }

    public Result(TResult result)
    {
        Value = result;
        Success = true;
    }
}
