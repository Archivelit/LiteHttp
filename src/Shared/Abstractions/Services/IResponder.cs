namespace LiteHttp.Abstractions;

public interface IResponder
{
    Task SendResponse(Socket connection, ReadOnlyMemory<byte> response);
}