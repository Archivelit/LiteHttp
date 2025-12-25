namespace LiteHttp.ObjectPooling;

/// <summary>
/// Represents a default object pool implementation that provides synchronous and asynchronous methods.
/// </summary>
public sealed class DefaultObjectPool<TObject> : ObjectPool<TObject> where TObject : class
{
    /// <summary>
    /// Creates a new instance of the <see cref="DefaultObjectPool{TObject}"/> class.
    /// </summary>
    public DefaultObjectPool() : base() { }

    /// <summary>
    /// Attempts to get an object from the pool.
    /// </summary>
    /// <param name="obj">Object obtained from the pool. Can be null if operation failed or pool is empty.</param>
    /// <returns><see langword="true"/> if an object was obtained; otherwise <see langword="false"/></returns>
    public bool TryGet([NotNullWhen(true)] out TObject? obj) => InternalTryGet(out obj);

    /// <summary>
    /// Returns an object to the pool.
    /// </summary>
    /// <param name="obj">Object that should be returned. If the object was obtained from elsewhere, no exception will be thrown.
    /// However this implementation is not optimized for such case</param>
    /// <returns><see langword="true"/> if an object was returned; otherwise <see langword="false"/></returns>
    public bool TryReturn(TObject obj) => InternalTryReturn(obj);

    /// <summary>
    /// Asynchronously gets an object from the pool if available.
    /// </summary>
    /// <remarks>If no objects available, method will wait when an object becomes available. Safe for concurrent use. 
    /// Null returned if operation canceled. Cancellation does not handled in this method, requires manual handling of
    /// <see cref="OperationCanceledException"/>.</remarks>
    /// <param name="obj">Object got from pool.</param>
    /// <returns>ValueTask that represents object reading from pool.</returns>
    public ValueTask<TObject> GetAsync(CancellationToken ct = default) => InternalGetAsync(ct);
    /// <summary>
    /// Asynchronously returns an object to the pool.
    /// </summary>
    /// <remarks>Safe for concurrent use. Cancellation does not handled in this method, requires manual handling of
    /// <see cref="OperationCanceledException"/>.</remarks>
    /// <param name="obj">Object that should be returned. If object got from other sources, no exception will be occured.</param>
    /// <returns>ValueTask that represents returning object to the pool.</returns>
    public ValueTask ReturnAsync(TObject obj, CancellationToken ct = default) => InternalReturnAsync(obj, ct);
}