namespace LiteHttp.Abstractions;

public interface IResponder
{
    ValueTask<int> SendResponse(Socket connection, ReadOnlyMemory<byte> response);
}