namespace LiteHttp.RequestProcessors;

public sealed class Responder : IResponder
{
    public async Task SendResponse(Socket connection, ReadOnlyMemory<byte> response) => 
        await connection.SendAsync(response).ConfigureAwait(false);
}