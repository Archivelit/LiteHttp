using System.Threading.Channels;
using LiteHttp.Abstractions;
using LiteHttp.Models.Events;

namespace LiteHttp.EventBus;

public sealed class RequestEventBus : IEventBus<RequestReceivedEvent>
{
    private Channel<RequestReceivedEvent> _channel = Channel.CreateUnbounded<RequestReceivedEvent>();

    public ValueTask PublishAsync(RequestReceivedEvent @event, CancellationToken ct = default) =>
        _channel.Writer.WriteAsync(@event, ct);

    public ValueTask<RequestReceivedEvent> ConsumeAsync(CancellationToken ct = default) =>
        _channel.Reader.ReadAsync(ct);
}