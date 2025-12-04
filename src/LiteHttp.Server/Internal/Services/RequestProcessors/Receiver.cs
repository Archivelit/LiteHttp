namespace LiteHttp.RequestProcessors;

internal sealed class Receiver : IReceiver
{
    public static readonly Receiver Instance = new();

    public async Task<Result<Memory<byte>>> RecieveFromConnection(Socket connection, CancellationToken ct)
    {
        try
        {
            using var owner = MemoryPool<byte>.Shared.Rent(4096);
            var buffer = owner.Memory;

            var receivedLength = await connection.ReceiveAsync(buffer, ct).ConfigureAwait(false);

            return new Result<Memory<byte>>(buffer[..receivedLength]);
        }
        catch (SocketException excpetion)
        {
            return new Result<Memory<byte>>(new Error(excpetion.ErrorCode, excpetion.Message));
        }
    }
}