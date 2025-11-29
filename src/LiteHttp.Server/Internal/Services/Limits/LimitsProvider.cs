namespace LiteHttp.Server.Services.Limits;

internal sealed class LimitsProvider : ILimitsProvider
{
    public static readonly LimitsProvider Default = new(new LimitsConfiguration(o => { }));

    public LimitsProvider(LimitsConfiguration configuration)
    {
        KeepAliveTimeout = configuration.KeepAliveTimeout;
        RequestHeadersTimeout = configuration.RequestHeadersTimeout;
        MaxConcurrentConnections = configuration.MaxConcurrentConnections;
        MaxConcurrentUpgradedConnections = configuration.MaxConcurrentUpgradedConnections;
        MaxRequestHeaderCount = configuration.MaxRequestHeaderCount;
        MaxResponseBufferSize = configuration.MaxResponseBufferSize;
        MaxRequestBufferSize = configuration.MaxRequestBufferSize;
        MaxRequestLineSize = configuration.MaxRequestLineSize;
        MaxRequestBodySize = configuration.MaxRequestBodySize;
        MaxRequestHeadersTotalSize = configuration.MaxRequestHeadersTotalSize;
    }

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