

namespace LiteHttp.Server;

public sealed class RequestEventBus : IEventBus<RequestReceivedEvent>
{
    private readonly Channel<RequestReceivedEvent> _channel = Channel.CreateUnbounded<RequestReceivedEvent>();

    public ValueTask PublishAsync(RequestReceivedEvent @event, CancellationToken ct = default) =>
        _channel.Writer.WriteAsync(@event, ct);

    public ValueTask<RequestReceivedEvent> ConsumeAsync(CancellationToken ct = default) =>
        _channel.Reader.ReadAsync(ct);
}