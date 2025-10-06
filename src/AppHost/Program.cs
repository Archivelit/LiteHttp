using AppHost;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<HttpListener>();

var host = builder.Build();
host.Run();
