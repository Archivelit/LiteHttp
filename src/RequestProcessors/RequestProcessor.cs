namespace LiteHttp.RequestProcessors;

public class RequestProcessor(
    IRequestParser parser
    ) : IRequestProcessor
{
    public async Task ProcessConnection(Socket connection, CancellationToken ct)
    {
        var request = await GetRequestContext(connection, ct);

        parser.Parse(request);
    }

    [SkipLocalsInit]
    private async Task<string> GetRequestContext(Socket connection, CancellationToken ct)
    {
        using var owner = MemoryPool<byte>.Shared.Rent(4096);
        var buffer = owner.Memory;
        
        var receivedLength = await connection.ReceiveAsync(buffer, ct);

        return Encoding.UTF8.GetString(buffer.Span.Slice(0, receivedLength));
    }
}