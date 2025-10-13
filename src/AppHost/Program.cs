var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSerilog();

builder.Services.AddSingleton<IEventBus<RequestReceivedEvent>, RequestEventBus>();
builder.Services.AddSingleton<IRouteResolver, RouteResolver>();
builder.Services.AddTransient<IRequestSerializer, RequestSerializer>();
builder.Services.AddTransient<IRequestParser, RequestParser>();

builder.Services.AddHostedService(sp => new Listener(sp.GetRequiredService<IEventBus<RequestReceivedEvent>>()));
builder.Services.AddHostedService(sp => new ServerWorker(
    sp.GetRequiredService<IRequestSerializer>(),
    sp.GetRequiredService<IRequestParser>(),
    sp.GetRequiredService<IRouteResolver>(),
    sp.GetRequiredService<IEventBus<RequestReceivedEvent>>()
    ));

var host = builder.Build();
host.Run();