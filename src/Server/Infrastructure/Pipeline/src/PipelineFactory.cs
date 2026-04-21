namespace LiteHttp.Pipeline;

public sealed class PipelineFactory
{
    public Func<IRouter> RouterFactory { get; set; }
    public Func<Parser> ParserFactory { get; set; }
    public Func<ResponseBuilder> ResponseBuilderFactory { get; set; }
    public Func<ActionInvoker> ActionInvokerFactory { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PipelineFactory(Action<PipelineFactory> factoryDelegate)
    {
        factoryDelegate(this);
        
        ThrowIfAnyFactoryIsNull();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private void ThrowIfAnyFactoryIsNull()
    {
        if (RouterFactory is null) throw new ArgumentNullException(nameof(RouterFactory));
        if (ParserFactory is null) throw new ArgumentNullException(nameof(ParserFactory));
        if (ResponseBuilderFactory is null) throw new ArgumentNullException(nameof(ResponseBuilderFactory));
        if (ActionInvokerFactory is null) throw new ArgumentNullException(nameof(ActionInvokerFactory));
    }

    public Pipeline Create() => new(this);
}   