namespace LiteHttp.Abstractions;

public interface IActionResult
{
    ResponseCode ResponseCode { get; init; }
}