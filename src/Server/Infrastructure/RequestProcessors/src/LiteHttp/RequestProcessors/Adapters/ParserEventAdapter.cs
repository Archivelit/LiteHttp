using LiteHttp.Models.LiteHttp.Models;

namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ParserEventAdapter
{
    private readonly Parser _parser = Parser.Instance;

    public void Handle(ConnectionContext connectionContext)
    {
        var result = _parser.Parse(connectionContext.Buffer);

        connectionContext.HttpContext = result.Value;

        OnParsed(connectionContext);
    }

    private event Action<ConnectionContext> Parsed;

    private void OnParsed(ConnectionContext c) => Parsed?.Invoke(c);
    
    public void SubscribeToParsed(Action<ConnectionContext> handler) => Parsed += handler;
    public void UnsubscribeParsed(Action<ConnectionContext> handler) => Parsed += handler;
}
