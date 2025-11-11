namespace LiteHttp.Models;

public record struct HttpContext(
    ReadOnlyMemory<byte> Method,
    ReadOnlyMemory<byte> Path,
    Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> Headers,
    ReadOnlyMemory<byte>? Body);