using LiteHttp.Helpers;

namespace LiteHttp.RequestProcessors.Adapters;

public sealed class ParserEventAdapter
{
    private readonly Parser _parser = Parser.Instance;

    public void ParseRequest(ConnectionContext connectionContext)
    {
        var buffer = connectionContext.SocketEventArgs.Buffer;
        var result = _parser.Parse(buffer);

        if (!result.Success) OnParsingError(connectionContext, InternalActionResults.BadRequest());
        
        connectionContext.HttpContext = result.Value;

        OnParsed(connectionContext);
    }

    private event Action<ConnectionContext> Parsed;
    private void OnParsed(ConnectionContext c) => Parsed?.Invoke(c);
    
    public void SubscribeToParsed(Action<ConnectionContext> handler) => Parsed += handler;
    public void UnsubscribeParsed(Action<ConnectionContext> handler) => Parsed += handler;
    
    
    private event Action<ConnectionContext, IActionResult> ParsingError;
    private void OnParsingError(ConnectionContext c, IActionResult result) => ParsingError?.Invoke(c, result);
    
    public void SubscribeToParsingError(Action<ConnectionContext, IActionResult> handler) => ParsingError += handler;
    public void UnsubscribParsingError(Action<ConnectionContext, IActionResult> handler) => ParsingError += handler;

}
