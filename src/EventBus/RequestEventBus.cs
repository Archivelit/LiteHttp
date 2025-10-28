using System.Threading.Channels;
using LiteHttp.Abstractions;
using LiteHttp.Models.Events;

namespace LiteHttp.EventBus;

public sealed class RequestEventBus : IEventBus<RequestReceivedEvent>
{
    private Channel<RequestReceivedEvent> _channel = Channel.CreateUnbounded<RequestReceivedEvent>();

    public async Task PublishAsync(RequestReceivedEvent @event, CancellationToken ct = default) =>
        await _channel.Writer.WriteAsync(@event, ct).ConfigureAwait(false);

    public async Task<RequestReceivedEvent> ConsumeAsync(CancellationToken ct = default) =>
        await _channel.Reader.ReadAsync(ct).ConfigureAwait(false);
}