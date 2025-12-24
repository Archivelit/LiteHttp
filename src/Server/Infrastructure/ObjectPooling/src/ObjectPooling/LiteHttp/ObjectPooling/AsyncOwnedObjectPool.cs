namespace LiteHttp.ObjectPooling;

public sealed class AsyncOwnedObjectPool<TObject> : ObjectPool<TObject> where TObject : class
{
    private readonly Func<TObject, ValueTask> _returnCallback;

    public AsyncOwnedObjectPool() : base() => _returnCallback = ReturnAsync;

    public bool TryGet([NotNullWhen(true)] out AsyncObjectOwner<TObject>? objectOwner)
    {
        var res = InternalTryGet(out var obj);

        if (res)
        {
            objectOwner = new AsyncObjectOwner<TObject>(obj!, _returnCallback);
            return true;
        }

        objectOwner = null;
        return false;
    }

    public async ValueTask<AsyncObjectOwner<TObject>> GetAsync()
    {
        var obj = await InternalGetAsync();
        return new AsyncObjectOwner<TObject>(obj, _returnCallback);
    }

    private ValueTask ReturnAsync(TObject obj) => InternalReturnAsync(obj);
}
