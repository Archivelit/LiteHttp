namespace LiteHttp.ObjectPooling.InitializationHelpers;

internal static class ObjectPoolInitializationHelper<TObject> where TObject : class
{
    public static void Initalize(int objectCount, ObjectPool<TObject> pool, Func<TObject> factory)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            pool._pool.Writer.TryWrite(factory());
        }
    }


    public static async ValueTask InitalizeAsync(int objectCount, ObjectPool<TObject> pool, Func<TObject> factory, CancellationToken cancellationToken = default)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool._pool.Writer.WriteAsync(factory(), cancellationToken).ConfigureAwait(false);
        }
    }

    public static async ValueTask InitalizeAsync(int objectCount, ObjectPool<TObject> pool, Func<ValueTask<TObject>> factory, CancellationToken cancellationToken = default)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool._pool.Writer.WriteAsync(await factory(), cancellationToken).ConfigureAwait(false);
        }
    }

    public static async Task InitializeAsync(int objectCount, ObjectPool<TObject> pool, Func<Task<TObject>> factory, CancellationToken cancellationToken = default)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool._pool.Writer.WriteAsync(await factory(), cancellationToken).ConfigureAwait(false);
        }
    }
}
