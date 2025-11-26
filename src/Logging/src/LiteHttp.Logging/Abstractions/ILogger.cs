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
    public void LogTrace(FormattableString message);
    public void LogDebug(FormattableString message);
    public void LogInformation(FormattableString message);
    public void LogWarning(FormattableString message);
    public void LogError(Exception ex, FormattableString message);
    public ILogger<TContext> ForContext<TContext>();
}

/// <inheritdoc/>
public interface ILogger<TCategoryName> : ILogger;