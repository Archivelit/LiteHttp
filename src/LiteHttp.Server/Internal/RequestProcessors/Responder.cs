namespace LiteHttp.RequestProcessors;

internal sealed class Responder : IResponder
{
    public static readonly Responder Instance = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<int> SendResponse(Socket connection, ReadOnlyMemory<byte> response) =>
        connection.SendAsync(response);
}