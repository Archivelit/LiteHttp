namespace LiteHttp.Models;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ActionResult(ResponseCode responseCode) : IActionResult
{
    public ResponseCode ResponseCode { get; } = responseCode;
}

[StructLayout(LayoutKind.Sequential)]
public record struct ActionResult<TResult>(
    ResponseCode ResponseCode,
    TResult Result
    ) : IActionResult<TResult>
{
    public ResponseCode ResponseCode { get; } = ResponseCode;
    public TResult Result { get; } = Result;
}