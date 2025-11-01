namespace LiteHttp.RequestProcessors;

public sealed class Responder : IResponder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task SendResponse(Socket connection, ReadOnlyMemory<byte> response) => 
        await connection.SendAsync(response).ConfigureAwait(false);
}