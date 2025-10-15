namespace LiteHttp.Models;

public record struct ActionResult(ResponseCode responseCode) : IActionResult
{
    public ResponseCode ResponseCode { get; } = responseCode;
}

public record struct ActionResult<TResult> (
    ResponseCode responseCode,
    TResult result
    ) : IActionResult<TResult>
{
    public ResponseCode ResponseCode { get; } = responseCode;
    public TResult Result { get; } = result;
} 