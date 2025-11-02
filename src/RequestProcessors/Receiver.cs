namespace LiteHttp.RequestProcessors;

public sealed class Receiver: IReceiver
{
    private readonly Pipe _pipe = new(new PipeOptions(MemoryPool<byte>.Shared, minimumSegmentSize: 2048));

    public async ValueTask<Memory<byte>> RecieveFromPipe(CancellationToken ct)
    {
        while (true)
        {
            var result = await _pipe.Reader.ReadAsync(ct).ConfigureAwait(false);

            var buffer = result.Buffer;

            if (!buffer.IsEmpty)
            {
                using var owner = MemoryPool<byte>.Shared.Rent((int)buffer.Length);
                var memory = owner.Memory.Slice(0, (int)buffer.Length);

                buffer.CopyTo(memory.Span);

                _pipe.Reader.AdvanceTo(buffer.End);

                return memory;
            }
            if (result.IsCompleted)
            {
                using var owner = MemoryPool<byte>.Shared.Rent((int)buffer.Length);
                var memory = owner.Memory.Slice(0, (int)buffer.Length);

                buffer.CopyTo(memory.Span);

                _pipe.Reader.AdvanceTo(buffer.End);

                return memory;
            }

            _pipe.Reader.AdvanceTo(buffer.Start, buffer.End);
        }
    }

    public async ValueTask<Memory<byte>> RecieveFromConnection(Socket connection, CancellationToken ct)
    {
        using var owner = MemoryPool<byte>.Shared.Rent(4096);
        var buffer = owner.Memory;

        var receivedLength = await connection.ReceiveAsync(buffer, ct).ConfigureAwait(false);

        return buffer.Slice(0, receivedLength);
    }
}