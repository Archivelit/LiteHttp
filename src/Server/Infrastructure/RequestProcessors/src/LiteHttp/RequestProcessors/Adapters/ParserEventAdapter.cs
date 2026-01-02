using HttpContext = LiteHttp.Models.HttpContext;

namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ParserEventAdapter
{
    private readonly Parser _parser = Parser.Instance;

    public void Handle(object? sender, Memory<byte> buffer)
    {
        var result = _parser.Parse(buffer);

        OnParsed(result.Value);
    }

    private event EventHandler<HttpContext> Parsed;

    private void OnParsed(HttpContext httpContext) => Parsed?.Invoke(this, httpContext);
    
    public void SubscribeParsed(EventHandler<HttpContext> handler) => Parsed += handler;
    public void UnsubscribeParsed(EventHandler<HttpContext> handler) => Parsed += handler;
}
