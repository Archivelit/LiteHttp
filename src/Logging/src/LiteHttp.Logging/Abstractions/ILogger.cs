namespace LiteHttp.Logging.Abstractions;

// TODO: improve documentation
/// <summary>
/// Default logger interface used by app
/// </summary>
/// <remarks>
/// Implement it if you want custom logger or logger provider
/// </remarks>
public interface ILogger
{
    void LogTrace(FormattableString message);
    void LogDebug(FormattableString message);
    void LogInformation(FormattableString message);
    void LogWarning(FormattableString message);
    void LogError(Exception ex, FormattableString message);
    ILogger<TContext> ForContext<TContext>();
}

/// <inheritdoc/>
public interface ILogger<TCategoryName> : ILogger;