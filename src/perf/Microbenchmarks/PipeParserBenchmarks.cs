namespace LiteHttp.Microbenchmarks;

[CPUUsageDiagnoser, MemoryDiagnoser, DotNetObjectAllocJobConfiguration, DotNetObjectAllocDiagnoser, CategoriesColumn, Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class PipeParserBenchmarks
{
    private readonly byte[] _request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");
    private RequestProcessors.PipeContext.Parser.Parser _parser;
    private Pipe _requestPipe;

    [GlobalSetup]
    public void Setup()
    {
        _parser = new();
    }

    [Params(10_000, 50_000, 1_000_000)]
    public int N;

    [IterationSetup]
    public void PipeSetup()
    {
        _requestPipe = new();

        FillPipe().GetAwaiter().GetResult();
    }

    [Benchmark, BenchmarkCategory("Parsing")]
    public async ValueTask Parse()
    {
        await _parser.Parse(_requestPipe);
    }

    private async Task FillPipe()
    {
        var writer = _requestPipe.Writer;

        var buffer = writer.GetSpan(_request.Length);
        _request.CopyTo(buffer);
        writer.Advance(_request.Length);

        await writer.FlushAsync();
        await writer.CompleteAsync();
    }
}