namespace LiteHttp.ObjectPooling;

public sealed class AsyncObjectOwner<Obj> : IAsyncDisposable where Obj : class
{
    private readonly Func<Obj, ValueTask> _returnCallback;
    public readonly Obj Object;

    public AsyncObjectOwner(Obj @object, Func<Obj, ValueTask> returnCallback)
    {
        Object = @object;
        _returnCallback = returnCallback;
    }

    public ValueTask DisposeAsync() => _returnCallback(Object);
}
