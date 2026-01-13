<!-- ![version](https://img.shields.io/badge/version-10.0.0-blue.svg) -->
# LiteHttp ![license](https://img.shields.io/badge/license-MIT-green.svg)

## Project Status: Archived ⚠️

This project is no longer in active development. It was built as a learning exercise to understand low-level networking, async I/O, and high-performance server architecture in C#.

**What was achieved:**

- Custom HTTP/1.1 server implementation without using `System.Net.Http`
- 28,000+ requests per second on commodity hardware
- Building concurrent lock-free systems using `ThreadPool`, concurrent collections and `Interlocked`
- Efficient parsing and response building using `Span<T>` and `Memory<T>` to minimize allocations
- Applying YAGNI principle: writing only code I needed, when I needed it
- Open-source development practices: MIT license, contribution guidelines, XML code documentation

**Why archived:**
I'm shifting focus to production-ready backend development (SQL, EF Core, REST APIs, microservices). This project taught me invaluable lessons about systems programming, but it's time to apply those skills to real-world backend work.

The code remains here as a reference and portfolio piece. I may return to add System.IO.Pipelines and HTTP keep-alive support in the future, but for now, it's feature-complete for its original learning goals.

**Key learnings:**
- How HTTP servers actually work under the hood
- Performance optimization and profiling techniques
- Lock-free concurrent data structures
- The value of knowing when NOT to over-optimize

Feel free to explore the code or reach out if you have questions!

# Getting Started

## Note

By default, LiteHttp listens on **localhost:30000**

## Quickstart

The following example shows a small app with one endpoint:

```csharp
using LiteHttp;

var builder = new ServerBuilder();

var server = builder.Build();

server.MapGet("/", () => ActionResults.Ok());

await server.Start();
```

You can also find it in `src/SampleApp/Program.cs` (It may differ from snippet above)

## Features

### Routing

LiteHttp provides AspNet Core minimal-api based routing.

Supported methods:

- Get
- Put
- Patch
- Post
- Delete

### Response building

Current version provides quite effecient response building.
The biggest problem is **logic customization**.
It is **minimal** or **completely absent**.

### Logging

LiteHttp uses own `ILogger` interface.
If you want to implement own logger, you have to work with `LiteHttp.Logging.Abstractions` package

### Logging libraries compatibility

If you want to use a logging library, you can check whether an **adapter** for your library is available `LiteHttp.Logging.Adapters.[libraryname]` or implement your **own adapter** if one does not exist

### Current Supported Libraries (via Adapter)

- Serilog

### Implementing own adapter

You can look at any adapter library to inspire or understand how it should be implemented

### You can find out more about [logging](./src/Logging)

### Listening address change

The default address and port can easily be changed using `builder.WithAddress()` and `builder.WithPort()` methods (Sample below)

```csharp
var builder = new ServerBuilder();

builder.WithAddress(new IPAddress([192, 168, 1, 1]));
builder.WithPort(8000);

var server = builder.Build();

server.MapGet("/", () => ActionResuls.Ok());

await server.Start();
```

In example above we setting up entire server on address `192.168.1.1:8000`

The `WithAddress` method also has overload `ServerBuilder WithAddress(string address)`. This overload maps domain using DNS (Domain Name System) to get first **IPV4** address for this domain

<!-- Update this section -->
# Benchmarks

**Note:** you can find out more about single component performance **[here](./tests/Benchmarks)**

### Average response time : **300 microseconds**

### Maximum reached short-term rps: **28500**

*Test was provided via **k6** on Machine with next configuration:*

> - CPU: Ryzen r7 5800X
> - RAM: 32GB 3200 MHz
> - OS: Windows 11

#### K6 file

```javascript
import http from 'k6/http';

export const options = {
  vus: 15,
  duration: '1m',
};


export default function () {
  const url = 'http://localhost:30000/';

  http.get(url);
}
```

# Limitations

LiteHttp is still under development.
Please, be aware that many certain features are missing or incomplete.
If you encounter an issue or have feature request, **feel free to open an issue**.

## Not supported

- Async endpoint delegates
- Endpoint delegates with parameters
- Automatic serialization of endpoint return values
- Custom header generation
- HttpContext access
- Middlewares or other analogs for server flow configuration
- Synchronous server start
- Integration with `Microsoft.Extensions.Hosting` or `Microsoft.Extensions.DependencyInjection`

Main goals:

- High optimization
- Aim for zero allocation
- Avoid using non-system libraries
- High configurationability
