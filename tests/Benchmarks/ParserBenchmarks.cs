namespace LiteHttp.Benchmarks;

[CPUUsageDiagnoser, MemoryDiagnoser, DotNetObjectAllocJobConfiguration, DotNetObjectAllocDiagnoser, CategoriesColumn, Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class ParserBenchmarks
{
    private readonly byte[] _request = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");
    private Parser _parser;

    [GlobalSetup]
    public void Setup()
    {
        _parser = new Parser();
    }

    [Params(1_000, 10_000, 1_000_000)]
    public int N;

    [Benchmark, BenchmarkCategory("Parsing")]
    public HttpContext Parse() =>
        _ = _parser.Parse(_request).Value;
}