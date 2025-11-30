namespace LiteHttp.Abstractions.Limits;

public interface ILimitsProvider
{
    public TimeSpan KeepAliveTimeout { get; }
    public TimeSpan RequestHeadersTimeout { get; }
    public long? MaxConcurrentConnections { get; }
    public long? MaxConcurrentUpgradedConnections { get; }
    public int MaxRequestHeaderCount { get; }
    public long? MaxResponseBufferSize { get; }
    public long? MaxRequestBufferSize { get; }
    public int MaxRequestLineSize { get; }
    public long? MaxRequestBodySize { get; }
    public int MaxRequestHeadersTotalSize { get; }
}