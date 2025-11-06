namespace LiteHttp.Logging;

public sealed class NullLogger : ILogger
{
    public static readonly NullLogger Instance = new NullLogger();

    public void LogCritical(FormattableString message) { }
    public void LogDebug(FormattableString message) { }
    public void LogError(FormattableString message) { }
    public void LogInformation(FormattableString message) { }
    public void LogTrace(FormattableString message) { }
    public void LogWarning(FormattableString message) { }
}

public sealed class NullLogger<TCategoryName> : ILogger<TCategoryName>
{
    public static readonly NullLogger<TCategoryName> Instance = new NullLogger<TCategoryName>();

    public void LogCritical(FormattableString message) { }
    public void LogDebug(FormattableString message) { }
    public void LogError(FormattableString message) { }
    public void LogInformation(FormattableString message) { }
    public void LogTrace(FormattableString message) { }
    public void LogWarning(FormattableString message) { }
}