using TestLib;

Test.SubscribeEventHandlers();
Test.InvokeEvent();

Console.WriteLine("Main method after event invoke");
Console.ReadKey();