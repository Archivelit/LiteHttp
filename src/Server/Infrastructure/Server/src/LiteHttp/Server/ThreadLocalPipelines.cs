namespace LiteHttp.Server;

internal static class ThreadLocalPipelines
{
    [ThreadStatic]
    public static Pipeline.Pipeline? Current;
}
