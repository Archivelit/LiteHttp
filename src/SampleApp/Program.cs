using LiteHttp.Server;

var server = new HttpServer();

server.MapGet("/", () => ActionResultFactory.Instance.Ok());

await server.Start();