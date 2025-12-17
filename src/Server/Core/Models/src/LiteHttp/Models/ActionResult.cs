namespace LiteHttp.Models;

public readonly struct ActionResult(ResponseCode responseCode) : IActionResult
{
    public ResponseCode ResponseCode { get; } = responseCode;
}

public record struct ActionResult<TResult>(
    ResponseCode ResponseCode,
    TResult Result
    ) : IActionResult<TResult>
{
    public ResponseCode ResponseCode { get; } = ResponseCode;
    public TResult Result { get; } = Result;
}