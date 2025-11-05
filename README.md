# LiteHttp ![version](https://img.shields.io/badge/version-1.0.10-blue.svg) ![license](https://img.shields.io/badge/license-MIT-green.svg)

- Lightweight and dependency-free (except Serilog)
- Built with System.* only
- Simple route mapping (MapGet, MapPost, etc.)
- ActionResultFactory for clean response handling
- Easily configurable host and port

# Getting Started

The following example shows easy server with one endpoint:

```csharp
var server = new HttpServer();

server.MapGet("/", () => ActionResultFactory.Instance.Ok());

await server.Start();
```

You can also find it in `src/SampleApp/Program.cs`

**Important that default server address is localhost:30000 (can be easily changed using `server.SetAddress()` and `server.SetPort()`)**

# Benchmarks

### Average response time: **7ms**

### Maximum stable short-term rps: **9300**

*Test was provided via **vegeta** on Machine with next configuration:*
> - CPU: Ryzen r7 5800X
> - RAM: 32GB 3200 MHz
> - OS: Windows 11

# License
This project is licensed under the MIT License.


# Contributing
Contributions, issues and feature requests are welcome!