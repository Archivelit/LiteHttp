using Serilog.Events;

namespace AppHost;

public static class LoggerInitializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        Configure(new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Is(GetMinimumLevel()));

    public static void Configure(LoggerConfiguration configuration) =>
        Log.Logger = configuration.CreateLogger();

    private static LogEventLevel GetMinimumLevel()
    {
#if DEBUG
        return LogEventLevel.Debug;
#else
        return LogEventLevel.Information;
#endif
    }
}