namespace LiteHttp.Models.Events;

public readonly record struct RequestReceivedEvent(Socket Connection) : IEvent
{
    public readonly Socket Connection = Connection;
}