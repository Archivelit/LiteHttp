# Benchmarks 

In this README file collected benchmark results of single LiteHttp components

## Info

You can reproduce them using `dotnet run -c Release` in the `Benchmarks` project

Benchmarks were run using BenchmarkDotNet v0.15.2, .NET 10.0, AMD Ryzen 7 5800X. More info:

```text
BenchmarkDotNet v0.15.2, Windows 11 (10.0.26200.6899)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.100-rc.2.25502.107
  [Host]     : .NET 10.0.0 (10.0.25.50307), X64 RyuJIT AVX2
  Job-NUUQIX : .NET 10.0.0 (10.0.25.50307), X64 RyuJIT AVX2
```

---

## Response generator

### ActionResult **without** body:

```text
| Method        | N       | Mean     | Error     | StdDev    | Gen0   | Allocated |
|-------------- |-------- |---------:|----------:|----------:|-------:|----------:|
| BuildResponse | 1000000 | 4.108 us | 0.0300 us | 0.0250 us | 0.0010 |      24 B |
| BuildResponse | 10000   | 4.144 us | 0.0305 us | 0.0271 us | 0.0014 |      24 B |
| BuildResponse | 1000    | 4.158 us | 0.0144 us | 0.0120 us | 0.0014 |      24 B |
```

### ActionResult with body was not tested due its **minimal realisation**. It **will** be tested on body realisation update

## Http parser

## Minimal Get request **without** body:

```text
| Method | N       | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------- |-------- |---------:|---------:|---------:|-------:|----------:|
| Parse  | 10000   | 13.83 us | 0.039 us | 0.034 us | 0.0362 |     616 B |
| Parse  | 1000    | 13.94 us | 0.068 us | 0.063 us | 0.0362 |     616 B |
| Parse  | 1000000 | 14.02 us | 0.128 us | 0.113 us | 0.0362 |     616 B |
```