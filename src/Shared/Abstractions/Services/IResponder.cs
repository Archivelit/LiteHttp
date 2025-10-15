namespace LiteHttp.Abstractions;

public interface IResponder
{
    Task SendResponse(Socket connection, string response);
}