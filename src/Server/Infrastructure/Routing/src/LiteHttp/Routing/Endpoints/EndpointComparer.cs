namespace LiteHttp.Routing;

internal sealed class EndpointComparer : IEqualityComparer<Endpoint>
{
    public static readonly EndpointComparer Instance = new();

    public bool Equals(Endpoint first, Endpoint second) =>
        first.Method.Length == second.Method.Length
        && first.Path.Length == second.Path.Length
        && first.Method.Span.SequenceEqual(second.Method.Span)
        && first.Path.Span.SequenceEqual(second.Path.Span);

    public int GetHashCode(Endpoint endpoint)
    {
        var hash = new HashCode();

        hash.Add(16843025);

        AddSpan(ref hash, endpoint.Method.Span);
        AddSpan(ref hash, endpoint.Path.Span);

        return hash.ToHashCode();
    }

    private void AddSpan(ref HashCode hash, ReadOnlySpan<byte> span)
    {
        hash.Add(span.Length);

        if (!Vector.IsHardwareAccelerated)
        {
            for (var i = 0; i < span.Length; i++)
                HashFunction(ref hash, span[i], span.Length);

            return;
        }

        int vectorWidth = Vector<byte>.Count;
        int index = 0;
        _ = span.Length % vectorWidth;

        for (; index + vectorWidth <= span.Length; index += vectorWidth)
        {
            var vector = new Vector<byte>(span.Slice(index, vectorWidth));
            var uintVector = Vector.AsVectorUInt32(vector);

            uintVector = ((uintVector << span.Length % 16) ^ (uintVector * 0x9E3779B1) ^ (uintVector >> 3)) & VectorConstants.HashVector;

            for (int i = 0; i < vectorWidth; i++)
                hash.Add(uintVector[i]);
        }

        for (; index < span.Length; index++)
            HashFunction(ref hash, span[index], span.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void HashFunction(ref HashCode hash, byte @byte, int spanLength) =>
        hash.Add(unchecked(((@byte << spanLength % 16) ^ (@byte * 0x9E3779B1) ^ (@byte >> 3)) & 0x9e3779b9));
}