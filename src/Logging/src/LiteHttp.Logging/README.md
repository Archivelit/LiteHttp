# LiteHttp Logging

**LiteHttp.Logging** is an abstract logging layer that allows you to use any logger or logging provider through a simple `ILogger` interface.
<br>
The goal is to provide a unified logging contract for LiteHttp and its dependent libraries, without being directly coupled to a specific implementation (Serilog, NLog, Microsoft.Extensions.Logging, etc.)

## Creating a Custom Logger

To add your own logger, simply **implement the `ILogger` interface** and (optionally) the generic version `ILogger<TCategoryName>`

### Quickstart

Create a new file, for example:

```
LiteHttp.Logging.Abstractions/CustomLogger.cs
```

with the following code

```csharp
using System;
using LiteHttp.Logging.Abstractions;

public sealed class ConsoleLogger : ILogger
{
    public void LogTrace(FormattableString message) =>
        Console.WriteLine($"[TRACE] {message}");

    public void LogDebug(FormattableString message) =>
        Console.WriteLine($"[DEBUG] {message}");

    public void LogInformation(FormattableString message) =>
        Console.WriteLine($"[INFO] {message}");

    public void LogWarning(FormattableString message) =>
        Console.WriteLine($"[WARN] {message}");

    public void LogError(Exception ex, FormattableString message) =>
        Console.WriteLine($"[ERROR] {message} — {ex.Message}");

    public ILogger<TContext> ForContext<TContext>() =>
        new ConsoleLogger<TContext>();
}

public sealed class ConsoleLogger<TContext> : ILogger<TContext>
{
    public void LogTrace(FormattableString message) =>
        Console.WriteLine($"[{typeof(TContext).Name}] [TRACE] {message}");

    // ... rest of the implementation is similar
    public ILogger<TNew> ForContext<TNew>() => new ConsoleLogger<TNew>();
}
```

---

## Adapter Sample

`LiteHttp.Logging.Adapters.Serilog` provides a ready-to-use integration with Serilog. [See source](https://github.com/Archivelit/LiteHttp/blob/main/src/Logging/src/Adapters/LiteHttp.Logging.Adapters.Serilog/SerilogLoggerAdapter.cs)

```csharp
using LiteHttp.Logging.Abstractions;
using Serilog;

public sealed class SerilogLoggerAdapter : ILogger
{
    public void LogTrace(FormattableString message) =>
        Log.Verbose(message.Format, message.GetArguments());

    public void LogDebug(FormattableString message) =>
        Log.Debug(message.Format, message.GetArguments());

    public void LogInformation(FormattableString message) =>
        Log.Information(message.Format, message.GetArguments());

    public void LogWarning(FormattableString message) =>
        Log.Warning(message.Format, message.GetArguments());

    public void LogError(Exception ex, FormattableString message) =>
        Log.Error(ex, message.Format, message.GetArguments());

    public ILogger<TContext> ForContext<TContext>() =>
        SerilogLoggerAdapter<TContext>.Instance;
}
```

See also: `SerilogLoggerAdapter<TCategoryName>` — a generic version that creates a scoped logger via `Log.ForContext<TCategoryName>()`

## Logger integration

To integrate the logger with the entire server, use the `HttpServer.AddLogger(ILogger logger)` method

```csharp
var server = new HttpServer(1);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var loggerAdapter = new SerilogLoggerAdapter();

server.AddLogger(loggerAdapter);
```

The sample above is taken from the [`SampleApp`](https://github.com/Archivelit/LiteHttp/tree/main/src/SampleApp) project.

### **Provided api may change and become out of sync with this README. If you notice any inconsistencies, please open an issue. Thank you!**

## Usage Sample

```csharp
var logger = new SerilogLoggerAdapter();

logger.LogInformation($"Starting HTTP client at {DateTime.UtcNow}");
try
{
    // core logic
}
catch (Exception ex)
{
    logger.LogError(ex, $"Request failed");
    throw;
}
```