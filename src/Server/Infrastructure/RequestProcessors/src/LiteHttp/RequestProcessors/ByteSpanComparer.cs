using System.Diagnostics.CodeAnalysis;

namespace LiteHttp.RequestProcessors;

internal static class ByteSpanComparerIgnoreCase
{
    public static bool Equals([DisallowNull] ReadOnlySpan<byte> first, [DisallowNull] ReadOnlySpan<byte> second)
    {
        // We do not do ReferenceEquals because this comparer not expected for usage
        // out of pipeline request handling context where references cannot be equivalent

        if (first.Length != second.Length) return false;

        for (int i = 0; i < first.Length; i++)
        {
            // 0x20 used to get upper-case letter from lower case
            if ((first[i] | 0x20) != (second[i] | 0x20))
                return false;
        }

        return true;
    }
    public static int GetHashCode([DisallowNull] ReadOnlySpan<byte> span)
    {
        var hashCode = new HashCode();
        for (var i = 0; i < span.Length; i++)
            hashCode.Add(span[i] | 0x20);

        return hashCode.ToHashCode();
    }
}