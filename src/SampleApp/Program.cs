using LiteHttp.Logging.Adapters.Serilog;
using LiteHttp.Server;
using Serilog;

var server = new HttpServer(1);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var loggerAdapter = new SerilogLoggerAdapter();

server.AddLogger(loggerAdapter);

server.MapGet("/", () => ActionResultFactory.Instance.Ok());

await server.Start();