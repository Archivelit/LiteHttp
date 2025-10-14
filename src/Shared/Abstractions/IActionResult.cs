namespace LiteHttp.Abstractions;

public interface IActionResult
{
    ResponseCode ResponseCode { get; init; }
}

public interface IActionResult<TResult> 
    : IActionResult 
    where TResult : class
{
    TResult Result { get; }
}