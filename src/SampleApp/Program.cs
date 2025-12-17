using LiteHttp;
using LiteHttp.Logging.Adapters.Serilog;

using Serilog;

namespace SampleApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = new ServerBuilder();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        var loggerAdapter = new SerilogLoggerAdapter();

        builder.WithLogger(loggerAdapter);

        var server = builder.Build();

        server.MapGet("/", ActionResults.Ok);

        await server.Start();
    }
}