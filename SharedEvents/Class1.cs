namespace SharedEvents;

public static class Events
{
    public static event EventHandler<string>? OnRequestRecieved;
    
    public static void RaiseOnStart()
    {
        OnStart?.Invoke(null, EventArgs.Empty);
    }
}