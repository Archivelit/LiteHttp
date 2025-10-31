namespace LiteHttp.RequestProcessors;

public sealed class RequestSerializer : IRequestSerializer
{
    public async Task<Memory<byte>> DeserializeFromConnectionAsync(Socket connection, Memory<byte> buffer, CancellationToken ct) =>
        await GetRequestContext(connection, buffer, ct).ConfigureAwait(false);

    [SkipLocalsInit]
    private async Task<Memory<byte>> GetRequestContext(Socket connection, Memory<byte> buffer, CancellationToken ct)
    {
        var receivedLength = await connection.ReceiveAsync(buffer, ct).ConfigureAwait(false);

        return buffer.Slice(0, receivedLength);
    }
}