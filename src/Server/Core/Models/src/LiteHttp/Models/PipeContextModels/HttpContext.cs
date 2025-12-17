namespace LiteHttp.Models.PipeContextModels;

public record struct HttpContext(
    ReadOnlyMemory<byte> Method,
    ReadOnlyMemory<byte> Route,
    Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> Headers,
    ReadOnlySequence<byte>? Body);