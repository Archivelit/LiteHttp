﻿namespace LiteHttp.RequestProcessors;

public sealed class Receiver: IReceiver
{
    public async Task<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct)
    {
        using var owner = MemoryPool<byte>.Shared.Rent(4096);
        var buffer = owner.Memory;

        var receivedLength = await connection.ReceiveAsync(buffer, ct).ConfigureAwait(false);

        return buffer.Slice(0, receivedLength);
    }
}