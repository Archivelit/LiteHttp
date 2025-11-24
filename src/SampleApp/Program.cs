//using LiteHttp.Logging.Adapters.Serilog;
using LiteHttp.Server;
//using Serilog;

var builder = new ServerBuilder();


////Log.Logger = new LoggerConfiguration()
////    .WriteTo.Console()
////    .MinimumLevel.Debug()
////    .CreateLogger();

//var loggerAdapter = new SerilogLoggerAdapter();

//builder.WithLogger(loggerAdapter);

var server = builder.Build();

server.MapGet("/", () => ActionResultFactory.Instance.Ok());

await server.Start();