namespace LiteHttp.Server;

#pragma warning disable CS8618, CS4014
public sealed class HttpServer : IServer, IDisposable
{
    private readonly InternalServer _internalServer;

    public HttpServer(int workersCount)
    {
        if (workersCount <= 1)
            _internalServer = new InternalServer(workersCount);
        else
            _internalServer = new InternalServer();
    }

    public Task Start(CancellationToken cancellationToken = default) =>
        _internalServer.Start(cancellationToken);

    public void SetPort(int port) => 
        _internalServer.SetPort(port);
    
    public void SetAddress(string address) =>
        _internalServer.SetAddress(address);
    
    public void MapGet(string route, Func<IActionResult> action) =>
        _internalServer.MapGet(route, action);
    
    public void MapDelete(string route, Func<IActionResult> action) =>
        _internalServer.MapDelete(route, action);

    public void MapPost(string route, Func<IActionResult> action) =>
        _internalServer.MapPost(route, action);

    public void MapPut(string route, Func<IActionResult> action) =>
        _internalServer.MapPut(route, action);

    public void MapPatch(string route, Func<IActionResult> action) =>
        _internalServer.MapPatch(route, action);
    
    public void Dispose() =>
        _internalServer.Dispose();
}