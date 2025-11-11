namespace LiteHttp.Extensions;

public static class StringExtensions
{
    public static ReadOnlyMemory<byte> AsMemoryByteArray(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return ReadOnlyMemory<byte>.Empty;

        var bytes = Encoding.UTF8.GetBytes(s);
        return new(bytes);
    }
}