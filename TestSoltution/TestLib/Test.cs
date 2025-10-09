namespace TestLib;

public static class Test
{
    public static event Func<Task>? TestingEventFlowEvent;

    public static async Task TestingEventFlowAction()
    {
        await Task.Delay(2000);
        
        Console.WriteLine("Event method");
    }

    public static void SubscribeEventHandlers()
    {
        TestingEventFlowEvent += TestingEventFlowAction;
    }

    public static void InvokeEvent()
    {
        TestingEventFlowEvent?.Invoke();
    }
}