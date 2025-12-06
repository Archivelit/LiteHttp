namespace LiteHttp.Abstractions;

public interface IResponder
{
    public ValueTask<Result<int>> SendResponse(Socket connection, ReadOnlyMemory<byte> response);
}