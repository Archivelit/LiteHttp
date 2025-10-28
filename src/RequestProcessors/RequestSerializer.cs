namespace LiteHttp.RequestProcessors;

public sealed class RequestSerializer : IRequestSerializer
{
    public async Task<string> DeserializeFromConnectionAsync(Socket connection, CancellationToken ct) =>
        await GetRequestContext(connection, ct).ConfigureAwait(false);

    [SkipLocalsInit]
    private async Task<string> GetRequestContext(Socket connection, CancellationToken ct)
    {
        using var owner = MemoryPool<byte>.Shared.Rent(4096);
        var buffer = owner.Memory;
        
        var receivedLength = await connection.ReceiveAsync(buffer, ct).ConfigureAwait(false);

        return Encoding.UTF8.GetString(buffer.Span.Slice(0, receivedLength));
    }
}