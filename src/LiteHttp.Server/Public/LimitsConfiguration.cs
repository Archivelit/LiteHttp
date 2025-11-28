// Source - https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/Core/src/KestrelServerLimits.cs
// Retrieved 27.11.2025 (DD.MM.YYYY), License - MIT license

namespace LiteHttp.Server;

// TODO: Add documentation
public class LimitsConfiguration
{
    public LimitsConfiguration(Action<LimitsConfiguration> callback) => callback.Invoke(this);

    public TimeSpan KeepAliveTimeout
    {
        get;
        set
        {
            if (value <= TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.PositiveTimeSpanRequired);

            field = value == Timeout.InfiniteTimeSpan
                ? TimeSpan.MaxValue
                : value;
        }
    } = TimeSpan.FromSeconds(130);

    public TimeSpan RequestHeadersTimeout
    {
        get;
        set
        {
            if (value <= TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.PositiveTimeSpanRequired);

            field = value == Timeout.InfiniteTimeSpan
                ? TimeSpan.MaxValue
                : value;
        }
    } = TimeSpan.FromSeconds(30);

    public long? MaxConcurrentConnections
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrNonNegativeNumberRequired);

            field = value;
        }
    }

    public long? MaxConcurrentUpgradedConnections
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrNonNegativeNumberRequired);

            field = value;
        }
    }

    public int MaxRequestHeaderCount
    {
        get;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 100;

    public long? MaxResponseBufferSize
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrNonNegativeNumberRequired);

            field = value;
        }
    } = 64 * 1024;

    public long? MaxRequestBufferSize
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrNonNegativeNumberRequired);

            field = value;
        }
    } = 1024 * 1024;

    public int MaxRequestLineSize
    {
        get;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 8 * 1024;

    public long? MaxRequestBodySize
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 30000000;

    public int MaxRequestHeadersTotalSize
    {
        get;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 32 * 1024;
}