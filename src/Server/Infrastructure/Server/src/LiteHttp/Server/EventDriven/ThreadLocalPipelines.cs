namespace LiteHttp.Server.EventDriven;

internal static class ThreadLocalPipelines
{
    [ThreadStatic]
    public static Pipeline.Pipeline? Current;
}
