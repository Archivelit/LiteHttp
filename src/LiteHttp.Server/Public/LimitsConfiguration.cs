// Based on: ASP.NET Core KestrelServerLimits
// Source: https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/Core/src/KestrelServerLimits.cs
// Retrieved: 2025-11-27
// License: MIT license

namespace LiteHttp.Server;

/// <summary>
/// <see cref="LimitsConfiguration"/> provides an api to configure limits of
/// the <see cref="HttpServer"/> instance. 
/// </summary>
public sealed class LimitsConfiguration
{
    /// <summary>
    /// Configure limits using callback.
    /// </summary>
    /// <param name="callback">
    /// A callback action that provides configuration.
    /// </param>
    public LimitsConfiguration(Action<LimitsConfiguration> callback) =>
        callback?.Invoke(this);

    /// <summary>
    /// Gets or sets keep-alive timeout.
    /// <para>
    /// Defaults to 130 seconds.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Can be set to <see cref="Timeout.InfiniteTimeSpan"/>.
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value less or equals <see cref="TimeSpan.Zero"/>.
    /// </exception>
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

    /// <summary>
    /// Gets or sets the request headers timeout.
    /// <para>
    /// Defaults to 30 seconds.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Can be set to <see cref="Timeout.InfiniteTimeSpan"/>.
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is  less or equals <see cref="TimeSpan.Zero"/>.
    /// </exception>
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

    /// <summary>
    /// Gets or sets the maximum of open concurrent connections.
    /// When set to null, the limit is disabled.
    /// <para>
    /// Defaults to null (unlimited open connections).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <param name="value">
    /// Non-zero positive integer.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero.
    /// </exception>
    public long? MaxConcurrentConnections
    {
        get;
        set
        {
            if (value is <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrPositiveNumberRequired);

            field = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of open, upgraded connections.
    /// When set to null, the number of upgraded connections is unlimited.
    /// An upgraded connection is one that has been switched from HTTP to another protocol, such as WebSockets.
    /// <para>
    /// Defaults to null.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is negative number.
    /// </exception>
    public long? MaxConcurrentUpgradedConnections
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullZeroOrPositiveNumberRequired);

            field = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum allowed count of headers in request.
    /// <para>
    /// Defaults to 100.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero.
    /// </exception> 
    public int MaxRequestHeaderCount
    {
        get;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 100;

    /// <summary>
    /// Gets or sets the maximum response buffer size.
    /// If set to null, disables buffer limits.
    /// <para>
    /// Defaults to 64 kB (65,536 bytes).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero.
    /// </exception>
    public long? MaxResponseBufferSize
    {
        get;
        set
        {
            if (value is <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrPositiveNumberRequired);

            field = value;
        }
    } = 64 * 1024;

    /// <summary>
    /// Gets or sets the maximum size of request buffer.
    /// If set to null, disables buffer limits.
    /// <para>
    /// Defaults to 1 MB (1,048,576 bytes).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero.
    /// </exception>
    public long? MaxRequestBufferSize
    {
        get;
        set
        {
            if (value is <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NullOrPositiveNumberRequired);

            field = value;
        }
    } = 1024 * 1024;

    /// <summary>
    /// Gets or sets maximum allowed size of request line.
    /// <para>
    /// Defaults to 8192 bytes (8 kB).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero. 
    /// </exception>
    public int MaxRequestLineSize
    {
        get;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 8 * 1024;

    /// <summary>
    /// Gets or sets maximum allowed request body size.
    /// <para>
    /// Defaults to 30 000 000 bytes (approximately 28,6 MB).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is negative number.
    /// </exception>
    public long? MaxRequestBodySize
    {
        get;
        set
        {
            if (value is < 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 30_000_000;

    /// <summary>
    /// Gets or sets maximum allowed size for HTTP request headers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not supported yet.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if value is less or equals zero 
    /// </exception>
    public int MaxRequestHeadersTotalSize
    {
        get;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

            field = value;
        }
    } = 32 * 1024;
}