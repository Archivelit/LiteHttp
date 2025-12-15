namespace LiteHttp.Server.Services.Comparers;

internal static class ByteSpanComparerIgnoreCase
{
    public static bool Equals([DisallowNull] ReadOnlySpan<byte> @this, [DisallowNull] ReadOnlySpan<byte> other)
    {
        // We do not do ReferenceEquals because this comparer not expected for usage
        // out of pipeline request handling context where references cannot be equivalent

        if (@this.Length != other.Length) return false;

        for (int i = 0; i < @this.Length; i++)
        {
            // 0x20 used to get upper-case letter from lower case
            if ((@this[i] | 0x20) != (other[i] | 0x20))
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