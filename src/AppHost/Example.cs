namespace AppHost;

public static class Example
{
    public static IActionResult<string> Foo() => new ActionResult<string>(ResponseCode.Ok,"Hello, World");
}