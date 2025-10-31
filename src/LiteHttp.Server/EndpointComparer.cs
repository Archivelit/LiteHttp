namespace LiteHttp.Server;

public class EndpointComparer : IEqualityComparer<Endpoint>
{
    public bool Equals(Endpoint first, Endpoint second) => 
        first.Method.Length == second.Method.Length 
        && first.Path.Length == second.Path.Length
        && first.Method.Span.SequenceEqual(second.Method.Span)
        && first.Path.Span.SequenceEqual(second.Path.Span);

    public int GetHashCode(Endpoint endpoint)
    {
        // REVIEW: algorithm can be stronger and vectorized
        var hash = new HashCode();

        AddSpan(ref hash, endpoint.Method.Span);
        AddSpan(ref hash, endpoint.Path.Span);
        
        return hash.ToHashCode();
    }

    private HashCode AddSpan(ref HashCode hash, ReadOnlySpan<byte> span)
    {
        hash.Add(span.Length);

        foreach (var val in span)
            hash.Add(val);

        return hash;
    }
}