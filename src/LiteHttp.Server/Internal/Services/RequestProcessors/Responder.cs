namespace LiteHttp.RequestProcessors;

internal sealed class Responder : IResponder
{
    public static readonly Responder Instance = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask<Result<int>> SendResponse(Socket connection, ReadOnlyMemory<byte> response) 
    {
        await connection.SendAsync(response);
        return new Result<int>(response.Length);
    }
}