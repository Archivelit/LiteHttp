namespace LiteHttp.RequestProcessors;

public sealed class Responder : IResponder
{
    private readonly Pipe _pipe = new();

    public ValueTask<FlushResult> SendResponseViaPipe(ReadOnlyMemory<byte> response, CancellationToken ct) =>
        _pipe.Writer.WriteAsync(response, ct);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<int> SendResponse(Socket connection, ReadOnlyMemory<byte> response) => 
        connection.SendAsync(response);
}