using LiteHttp.Logging.Abstractions;

using Serilog;

using ILogger = Serilog.ILogger;

namespace LiteHttp.Logging.Adapters.Serilog;

public sealed class SerilogLoggerAdapter : LiteHttp.Logging.Abstractions.ILogger
{
    /*public SerilogLoggerAdapter(LoggerConfiguration configuration)
    {
        Log.Logger = configuration.CreateLogger();
    }*/

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

    public ILogger<TContext> ForContext<TContext>() => 
        SerilogLoggerAdapter<TContext>.Instance;
}

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

    public ILogger<TContext> ForContext<TContext>() => 
        SerilogLoggerAdapter<TContext>.Instance;
}