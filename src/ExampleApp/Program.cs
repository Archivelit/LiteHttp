using LiteHttp.Enums;
using LiteHttp.Models;
using LiteHttp.Server;

var server = new HttpServer(3);

server.MapGet("/", () => new ActionResult(ResponseCode.Ok));

await server.Start(CancellationToken.None);