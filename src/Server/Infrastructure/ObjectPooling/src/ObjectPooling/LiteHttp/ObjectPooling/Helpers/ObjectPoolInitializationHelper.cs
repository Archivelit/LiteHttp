namespace LiteHttp.ObjectPooling.Helpers;

/// <summary>
/// A helper class for initializing object pools with pre-created objects.
/// </summary>
public static class ObjectPoolInitializationHelper<TObject> where TObject : class
{
    /// <summary>
    /// Initializes the specified object pool by pre-populating it with a given number of objects created by the
    /// provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and is not thread safe. Ensure that the
    /// pool is empty or in a valid state before calling this method.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    public static void Initialize(int objectCount, [DisallowNull] ObjectPool<TObject> pool, 
        [DisallowNull] Func<TObject> factory)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            pool.InternalTryReturn(factory());
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async ValueTask InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool, 
        [DisallowNull] Func<TObject> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(factory(), ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async ValueTask InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool, 
        [DisallowNull] Func<ValueTask<TObject>> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(await factory(), ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async Task InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool, [DisallowNull] 
    Func<Task<TObject>> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(await factory(), ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async ValueTask InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool,
        [DisallowNull] Func<CancellationToken, TObject> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(factory(ct), ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async ValueTask InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool,
        [DisallowNull] Func<CancellationToken, ValueTask<TObject>> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(await factory(ct), ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously initializes the specified object pool by pre-populating it with a given number of objects created 
    /// by the provided factory function.
    /// </summary>
    /// <remarks>This method adds objects to the pool sequentially and does not use parallelization to ensure
    /// thread safety. If the operation is canceled via the provided cancellation token, not all objects may be added to
    /// the pool.</remarks>
    /// <param name="objectCount">The number of objects to create and add to the pool. Must be non-negative.</param>
    /// <param name="pool">The object pool to be initialized with new objects.</param>
    /// <param name="factory">A function used to create new instances of the pooled object type.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the initialization operation.</param>
    /// <returns>A ValueTask that represents the asynchronous initialization operation.</returns>
    public static async Task InitializeAsync(int objectCount, [DisallowNull] ObjectPool<TObject> pool, [DisallowNull]
    Func<CancellationToken, Task<TObject>> factory, CancellationToken ct = default)
    {
        Debug.Assert(factory is not null);
        Debug.Assert(pool is not null);
        Debug.Assert(objectCount > 0);

        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(objectCount);

        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool.InternalReturnAsync(await factory(ct), ct).ConfigureAwait(false);
        }
    }
}
