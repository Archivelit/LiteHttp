namespace LiteHttp.Models.Events;

public record RequestReceivedEvent(Socket Connection) : IEvent;