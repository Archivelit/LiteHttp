namespace LiteHttp.Routing;

internal readonly struct Endpoint(ReadOnlyMemory<byte> path, ReadOnlyMemory<byte> method)
{
    public readonly ReadOnlyMemory<byte> Path = path;
    public readonly ReadOnlyMemory<byte> Method = method;
}