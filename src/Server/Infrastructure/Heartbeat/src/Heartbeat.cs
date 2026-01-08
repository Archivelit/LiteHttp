// Based on: ASP.NET Core Kestrel Heartbeat
// Source: https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/Core/src/Internal/Infrastructure/Heartbeat.cs
// Retrieved: 2026-01-06
// License: MIT license

using System.Diagnostics;

using LiteHttp.Logging.Abstractions;

namespace LiteHttp.Heartbeat;

public sealed class Heartbeat : IDisposable
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);
    private const string NoHandlersExceptionString = "Heartbeat not needed to be initialized if here is no handlers in app"; 
    
    private readonly Action[] _callbacks;
    private readonly ManualResetEventSlim _timer = new ManualResetEventSlim(false, 0);
    private readonly Thread _heartbeatThread;
    private readonly ILogger<Heartbeat> _logger;
    
    public Heartbeat(Span<IHeartbeatHandler> heartbeatHandlers, ILogger<Heartbeat> logger)
    {
        Debug.Assert(heartbeatHandlers.Length > 0, NoHandlersExceptionString);

        ArgumentException.ThrowIfNullOrEmpty(NoHandlersExceptionString);
        
        _logger = logger;
        
        _callbacks = new Action[heartbeatHandlers.Length];
        
        for (int i = 0; i < heartbeatHandlers.Length; i++) 
            _callbacks[i] = heartbeatHandlers[i].OnHeartbeat;
        
        _heartbeatThread = new Thread(Loop)
        {
            IsBackground = true,
            Name = "Heartbeat"
        };

        _heartbeatThread.Start();
    }
    
    private void OnHeartbeat()
    {
        foreach (var callback in _callbacks)
        {
            try
            {
                callback();
                // optional: detect long heartbeat
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"Exception thrown in heartbeat handler");
            }
        }
    }
    
    private void Loop()
    {
        while (!_timer.Wait(Interval))
            OnHeartbeat();
    }

    public void Dispose()
    {
        _timer.Set();
        
        if (_heartbeatThread.IsAlive)
            _heartbeatThread.Join();

        _timer.Dispose();
    }
}
