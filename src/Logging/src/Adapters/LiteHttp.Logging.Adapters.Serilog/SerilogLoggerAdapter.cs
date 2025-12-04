using LiteHttp.Logging.Abstractions;

using Serilog;

using ILogger = Serilog.ILogger;

namespace LiteHttp.Logging.Adapters.Serilog;

/// <summary>
/// Logger adapter for <c>Serilog</c> logger.
/// Implements <see cref="Abstractions.ILogger"/> so can be easily/.
/// </summary>
public sealed class SerilogLoggerAdapter : LiteHttp.Logging.Abstractions.ILogger
{
    /// <summary>
    /// Creates logger with provided configuration and replaces
    /// <see cref="Log.Logger"/> by logger with new configuration. 
    /// </summary>
    /// <param name="configuration"><see cref="LoggerConfiguration"/> for logger</param>
    public SerilogLoggerAdapter(LoggerConfiguration configuration) =>
        Log.Logger = configuration.CreateLogger();

    public SerilogLoggerAdapter() { }

    public void LogTrace(FormattableString message) =>
        Log.Verbose(message.Format, message.GetArguments());

    public void LogDebug(FormattableString message) =>
        Log.Debug(message.Format, message.GetArguments());

    public void LogInformation(FormattableString message) =>
        Log.Information(message.Format, message.GetArguments());

    public void LogWarning(FormattableString message) =>
        Log.Warning(message.Format, message.GetArguments());

    public void LogError(Exception ex, FormattableString message) =>
        Log.Error(message.Format, message.GetArguments());

    public void LogError(FormattableString message) => 
        Log.Error(message.Format, message.GetArguments());

    public ILogger<TContext> ForContext<TContext>() =>
        SerilogLoggerAdapter<TContext>.Instance;
}

/// <summary>
/// Represents a category-specific <see cref="Serilog"/> logger, typically bound to the type
/// where the logging occurs.
/// </summary>
/// <typeparam name="TCategoryName">
/// The type defining the logger category.
/// </typeparam>
public sealed class SerilogLoggerAdapter<TCategoryName> : LiteHttp.Logging.Abstractions.ILogger<TCategoryName>
{
    public static readonly SerilogLoggerAdapter<TCategoryName> Instance = new();
    private static readonly ILogger Logger = Log.ForContext<TCategoryName>();

    public void LogTrace(FormattableString message) =>
        Logger.Verbose(message.Format, message.GetArguments());

    public void LogDebug(FormattableString message) =>
        Logger.Debug(message.Format, message.GetArguments());

    public void LogInformation(FormattableString message) =>
        Logger.Information(message.Format, message.GetArguments());

    public void LogWarning(FormattableString message) =>
        Logger.Warning(message.Format, message.GetArguments());

    public void LogError(Exception ex, FormattableString message) =>
        Logger.Error(message.Format, message.GetArguments());

    public void LogError(FormattableString message) =>
        Logger.Error(message.Format, message.GetArguments());

    public ILogger<TContext> ForContext<TContext>() =>
        SerilogLoggerAdapter<TContext>.Instance;
}