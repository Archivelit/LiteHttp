namespace LiteHttp.Microbenchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<PipeParserBenchmarks>();
    }
}