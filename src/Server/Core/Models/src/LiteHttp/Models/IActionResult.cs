namespace LiteHttp.Models;

public interface IActionResult
{
    public ResponseCode ResponseCode { get; }
}

public interface IActionResult<TResult>
    : IActionResult
{
    public TResult Result { get; }
}