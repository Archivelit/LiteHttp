using LiteHttp.Enums;
using LiteHttp.Models;
using LiteHttp.Server;

var server = new HttpServer(8);

server.MapGet("/", () => new ActionResult(ResponseCode.Ok));

await server.Start();