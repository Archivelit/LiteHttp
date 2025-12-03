namespace LiteHttp.Models;

public readonly record struct Result<TResult>
{
    public Error? Error { get; }
    public TResult? Value { get; }
    public bool Success { get; }

    public Result(Error error)
    {
        Error = error;
        Success = false;
    }

    public Result(TResult result)
    {
        Value = result;
        Success = true;
    }
}