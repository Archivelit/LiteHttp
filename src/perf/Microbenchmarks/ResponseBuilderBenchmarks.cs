namespace LiteHttp.Microbenchmarks;

[CPUUsageDiagnoser, MemoryDiagnoser, DotNetObjectAllocJobConfiguration, DotNetObjectAllocDiagnoser, CategoriesColumn, Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class ResponseBuilderBenchmarks
{
    private ResponseBuilder _responseBuilder;

    [GlobalSetup]
    public void Setup()
    {
        _responseBuilder = new ResponseBuilder
        {
            Address = "localhost",
            Port = 8080
        };
    }

    [Params(1_000, 10_000, 1_000_000)]
    public int N;

    [Benchmark, BenchmarkCategory("ResponseBuilding")]
    public ReadOnlyMemory<byte> BuildResponse() =>
       _ = _responseBuilder.Build(InternalActionResults.Ok());
}