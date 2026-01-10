namespace LiteHttp.Models;

[StructLayout(LayoutKind.Sequential)]
public record struct HttpContext(
    ReadOnlyMemory<byte> Method,
    ReadOnlyMemory<byte> Route,
    ReadOnlyMemory<byte>? Body,
    Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> Headers);