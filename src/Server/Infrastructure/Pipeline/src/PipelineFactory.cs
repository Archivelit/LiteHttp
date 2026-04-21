namespace LiteHttp.Pipeline;

#nullable disable
public sealed class PipelineFactory
{
    public Func<IRouter> RouterFactory { get; set; }
    public Func<Parser> ParserFactory { get; set; }
    public Func<ResponseBuilder> ResponseBuilderFactory { get; set; }
    public Func<ActionInvoker> ActionInvokerFactory { get; set; }

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
        if (ActionInvokerFactory is null) throw new ArgumentNullException(nameof(ActionInvokerFactory));
    }

    public Pipeline Create() => new(this);
}   