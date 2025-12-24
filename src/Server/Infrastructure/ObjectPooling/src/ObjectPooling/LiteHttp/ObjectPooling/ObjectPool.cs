namespace LiteHttp.ObjectPooling;

/// <summary>
/// Provides a base class for managing a pool of reusable objects of a specified reference type.
/// </summary>
/// <remarks>This class is intended to be inherited by concrete pool implementations that define 
/// specific pooling policies. The pool is thread-safe and can be used concurrently by
/// multiple threads. Internal channel can be accessed in inherited classes.</remarks>
/// <typeparam name="TObject">The type of objects to be managed by the pool. Must be a reference type.</typeparam>
public abstract class ObjectPool<TObject> where TObject : class
{
    /// <summary>
    /// Provides access to the underlying channel used for pooling objects of type TObject.
    /// </summary>
    /// <remarks>Intended for use by derived classes to interact directly with the object pool's channel. The
    /// channel manages the storage and retrieval of pooled objects in a thread-safe manner.</remarks>
    protected internal readonly Channel<TObject> _pool;

    /// <summary>
    /// Creates a new instance of the ObjectPool class, initializing the internal channel for object pooling.
    /// </summary>
    public ObjectPool() => _pool = Channel.CreateUnbounded<TObject>();

    /// <summary>
    /// Synchronously attempts to get an object from the pool. Recommended for use in TryGet implementations 
    /// to ensure compatibility with the latest API version
    /// </summary>
    /// <param name="obj">Object read from pool. Can be null if operation failed or pool is empty.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/></returns>
    protected internal bool InternalTryGet([NotNullWhen(true)] out TObject? obj) => _pool.Reader.TryRead(out obj);

    /// <summary>
    /// Synchronously attempts to return an object to the pool. Recommended for use in TryReturn implementations 
    /// to ensure compatibility with the latest API version
    /// </summary>
    /// <param name="obj">Object to return back to pool.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/></returns>
    protected internal bool InternalTryReturn(TObject obj) => _pool.Writer.TryWrite(obj);

    /// <summary>
    /// Asynchronously gets an object from the pool. Recommended for use in GetAsync implementations to 
    /// ensure compatibility with the latest API version.
    /// </summary>
    /// <returns>ValueTask that represents object reading from pool.</returns>
    protected internal ValueTask<TObject> InternalGetAsync() => _pool.Reader.ReadAsync();

    /// <summary>
    /// Asynchronously returns an object to the pool. Recommended for use in ReturnAsync implementations to
    /// ensure compatibility with the latest API version.
    /// </summary>
    /// <param name="obj">Object to return back to pool.</param>
    /// <returns>ValueTask that represents returning object to the pool.</returns>
    protected internal ValueTask InternalReturnAsync(TObject obj) => _pool.Writer.WriteAsync(obj);
}