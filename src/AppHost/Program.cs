var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSerilog();

builder.Services.AddSingleton<IEventBus<RequestReceivedEvent>, RequestEventBus>();
builder.Services.AddSingleton<IRouter, Router>();
builder.Services.AddTransient<IRequestSerializer, RequestSerializer>();
builder.Services.AddTransient<IRequestParser, RequestParser>();
builder.Services.AddTransient<IResponseGenerator, ResponseGenerator>();
builder.Services.AddTransient<IResponder, Responder>();

builder.Services.AddHostedService(sp => new Listener(sp.GetRequiredService<IEventBus<RequestReceivedEvent>>()));
builder.Services.AddHostedService(sp => new ServerWorker(
    sp.GetRequiredService<IRequestSerializer>(),
    sp.GetRequiredService<IRequestParser>(),
    sp.GetRequiredService<IRouter>(),
    sp.GetRequiredService<IResponseGenerator>(),
    sp.GetRequiredService<IResponder>(),
    sp.GetRequiredService<IEventBus<RequestReceivedEvent>>()
    ));

var host = builder.Build();
host.Run();