namespace LiteHttp.RequestProcessors;

public sealed class Responder : IResponder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<int> SendResponse(Socket connection, ReadOnlyMemory<byte> response) => 
        connection.SendAsync(response);
}