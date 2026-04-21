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
    private const string NoHandlersString = "Heartbeat handlers were not initialized"; 
    
    private readonly Action[] _callbacks;
    private readonly ManualResetEventSlim _timer = new ManualResetEventSlim(false, 0);
    private readonly Thread _heartbeatThread;
    private readonly ILogger<Heartbeat> _logger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Heartbeat(IHeartbeatHandler[] heartbeatHandlers, ILogger<Heartbeat> logger)
    {
        if (heartbeatHandlers.Length == 0)
        {
            logger.LogWarning($"{NoHandlersString}");
            _logger = logger;
            return; // we don't have any handlers, so we won't start the heartbeat thread
        }
        
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
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    
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
                _logger.LogError(ex, $"Exception thrown in heartbeat handler");
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
