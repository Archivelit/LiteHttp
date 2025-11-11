namespace LiteHttp.Abstractions;

public interface IActionResult
{
    ResponseCode ResponseCode { get; }
}

public interface IActionResult<TResult> 
    : IActionResult
{
    TResult Result { get; }
}