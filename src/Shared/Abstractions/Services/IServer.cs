namespace LiteHttp.Abstractions;

public interface IServer
{
    Task Start(CancellationToken cancellationToken);
    void SetPort(int port);
    void SetAddress(string address);
}