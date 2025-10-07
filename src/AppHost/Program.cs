var builder = Host.CreateApplicationBuilder(args);

LoggerInitializer.Initialize();
builder.Logging.AddSerilog();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();