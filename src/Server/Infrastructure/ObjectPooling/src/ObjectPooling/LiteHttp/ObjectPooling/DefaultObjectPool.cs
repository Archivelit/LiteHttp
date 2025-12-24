namespace LiteHttp.ObjectPooling;

public sealed class DefaultObjectPool<TObject> : ObjectPool<TObject> where TObject : class
{
    public DefaultObjectPool() : base() { }

    public bool TryGet([NotNullWhen(true)] out TObject? obj) => InternalTryGet(out obj);
    public bool TryReturn(TObject obj) => InternalTryReturn(obj);
    public ValueTask<TObject> GetAsync() => InternalGetAsync();
    public ValueTask ReturnAsync(TObject obj) => InternalReturnAsync(obj);
}