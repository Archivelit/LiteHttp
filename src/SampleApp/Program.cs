using System.Net;
using LiteHttp.Logging.Adapters.Serilog;
using LiteHttp.Server;
using Serilog;

var builder = new ServerBuilder();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var loggerAdapter = new SerilogLoggerAdapter();

builder.WithLogger(loggerAdapter);

builder.WithAddress(new IPAddress([192, 168, 1, 1]));
builder.WithPort(8000);

var server = builder.Build();

server.MapGet("/", () => ActionResultFactory.Instance.Ok());

await server.Start();