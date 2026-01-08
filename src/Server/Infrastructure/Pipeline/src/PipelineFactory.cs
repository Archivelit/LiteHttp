namespace LiteHttp.Pipeline;

#nullable disable
public sealed class PipelineFactory
{
    public Func<IRouter> RouterFactory { get; set; }
    public Func<Parser> ParserFactory { get; set; }
    public Func<SaeaResponseBuilder> ResponseBuilderFactory { get; set; }
    public Func<Executor> ExecutorFactory { get; set; }

    public PipelineFactory(Action<PipelineFactory> factoryDelegate)
    {
        factoryDelegate(this);
        
        ThrowIfAnyFactoryIsNull();
    }

    private void ThrowIfAnyFactoryIsNull()
    {
        if (RouterFactory is null) throw new ArgumentNullException(nameof(RouterFactory));
        if (ParserFactory is null) throw new ArgumentNullException(nameof(ParserFactory));
        if (ResponseBuilderFactory is null) throw new ArgumentNullException(nameof(ResponseBuilderFactory));
        if (ExecutorFactory is null) throw new ArgumentNullException(nameof(ExecutorFactory));
    }

    public Pipeline BuildPipeline() => new(this);
}   