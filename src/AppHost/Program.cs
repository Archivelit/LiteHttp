var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSerilog();

builder.Services.AddHostedService<Listener>();

var host = builder.Build();
host.Run();