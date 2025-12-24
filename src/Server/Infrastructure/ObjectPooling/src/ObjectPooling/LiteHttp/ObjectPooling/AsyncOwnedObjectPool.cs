namespace LiteHttp.ObjectPooling;

/// <summary>
/// Provides an asynchronous object pool that returns owned handles for pooled objects, enabling asynchronous return and
/// lifetime management of objects.
/// </summary>
/// <remarks>Use this class when pooled objects require asynchronous cleanup or when object return operations may 
/// be asynchronous. This class is thread-safe and suitable for concurrent use.</remarks>
/// <typeparam name="TObject">The type of objects to pool. Must be a reference type.</typeparam>
public sealed class AsyncOwnedObjectPool<TObject> : ObjectPool<TObject> where TObject : class
{
    /// <summary>
    /// A callback to return the object to the pool asynchronously. Always passed to returned object owners.
    /// </summary>
    private readonly Func<TObject, ValueTask> _returnCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncOwnedObjectPool{TObject}"/> class.
    /// </summary>
    public AsyncOwnedObjectPool() : base() => _returnCallback = ReturnAsync;

    /// <summary>
    /// Synchronously returns an owned object from the pool if available.
    /// </summary>
    /// <param name="objectOwner">Returned object owner. Can be null if operation failed.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/></returns>
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

    /// <summary>
    /// Asynchronously gets an owned object from the pool.
    /// </summary>
    /// <returns>ValueTask that represents get operatoin.</returns>
    public async ValueTask<AsyncObjectOwner<TObject>> GetAsync()
    {
        var obj = await InternalGetAsync();
        return new AsyncObjectOwner<TObject>(obj, _returnCallback);
    }

    /// <summary>
    /// Asynchronously returns an object to the pool.
    /// </summary>
    /// <remarks>
    /// Used only for callback from <see cref="AsyncObjectOwner{Obj}"/>.
    /// </remarks>
    /// <param name="obj">An object that must be returned to the pool.</param>
    /// <returns>ValueTask that represents return operation.</returns>
    private ValueTask ReturnAsync(TObject obj) => InternalReturnAsync(obj);
}
