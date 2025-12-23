namespace LiteHttp.RequestProcessors.PipeContext.Parser;

internal class HeaderComparer : IEqualityComparer<ReadOnlyMemory<byte>>
{
    public static readonly HeaderComparer Instance = new();

    public bool Equals(ReadOnlyMemory<byte> f, ReadOnlyMemory<byte> s) => ByteSpanComparerIgnoreCase.Equals(f.Span, s.Span);
    public int GetHashCode([DisallowNull] ReadOnlyMemory<byte> obj) => ByteSpanComparerIgnoreCase.GetHashCode(obj.Span);
}
