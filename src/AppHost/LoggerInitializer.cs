namespace AppHost;

public static class LoggerInitializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        Initialize(new LoggerConfiguration()
            .WriteTo.Console());

    public static void Initialize(LoggerConfiguration configuration) =>
        Log.Logger = configuration.CreateLogger();
}