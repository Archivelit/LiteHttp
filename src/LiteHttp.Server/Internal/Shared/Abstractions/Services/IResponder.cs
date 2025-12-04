namespace LiteHttp.Abstractions;

public interface IResponder
{
    public Task<Result<int>> SendResponse(Socket connection, ReadOnlyMemory<byte> response);
}