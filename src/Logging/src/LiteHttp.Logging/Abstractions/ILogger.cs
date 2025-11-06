namespace LiteHttp.Logging.Abstractions;

public interface ILogger
{
    void LogTrace(FormattableString message);
    void LogDebug(FormattableString message);
    void LogInformation(FormattableString message);
    void LogWarning(FormattableString message);
    void LogError(FormattableString message);
    void LogCritical(FormattableString message);
}

public interface ILogger<TCategoryName> : ILogger;