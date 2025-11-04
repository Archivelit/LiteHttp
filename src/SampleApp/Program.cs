using LiteHttp.Enums;
using LiteHttp.Models;
using LiteHttp.Server;

var server = new HttpServer();

server.MapGet("/", () => new ActionResult(ResponseCode.Ok));

await server.Start();