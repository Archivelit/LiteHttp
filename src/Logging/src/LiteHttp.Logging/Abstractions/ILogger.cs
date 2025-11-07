namespace LiteHttp.Logging.Abstractions;

public interface ILogger
{
    void LogTrace(FormattableString message);
    void LogDebug(FormattableString message);
    void LogInformation(FormattableString message);
    void LogWarning(FormattableString message);
    void LogError(Exception ex, FormattableString message);
    ILogger<TContext> ForContext<TContext>();
}

public interface ILogger<TCategoryName> : ILogger;