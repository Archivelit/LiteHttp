namespace LiteHttp.RequestProcessors.PipeContext.Parser;

public sealed class HeaderCollection
{
    private const int InitialCapacity = 8;
    public Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> Headers { get; } = new(InitialCapacity, HeaderComparer.Instance);

    public Result TryAdd(in ReadOnlyMemory<byte> name, in ReadOnlyMemory<byte> value)
    {
        if (!Headers.TryAdd(name, value))
            return HeaderParsingErrors.TwoSameHeaders;

        return Result.Successful;
    }
}
