namespace LiteHttp.Abstractions;

public interface IResponder
{
    public ValueTask<int> SendResponse(Socket connection, ReadOnlyMemory<byte> response);
}