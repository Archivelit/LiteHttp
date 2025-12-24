namespace LiteHttp.ObjectPooling.InitializationHelpers;

internal static class ObjectPoolInitializationHelper<TObject> where TObject : class
{
    public static void Initalize(int objectCount, ObjectPool<TObject> pool, Func<TObject> factory)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            var obj = CreateObject(factory);
            pool._pool.Writer.TryWrite(obj);
        }
    }

    public static async ValueTask InitalizeAsync(int objectCount, ObjectPool<TObject> pool, Func<ValueTask<TObject>> factory, CancellationToken cancellationToken = default)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            TObject obj;
            try
            {
                obj = await factory().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                throw; // Unreachable, but required to satisfy the compiler
            }
            await pool._pool.Writer.WriteAsync(obj, cancellationToken).ConfigureAwait(false);
        }
    }

    public static async ValueTask InitalizeAsync(int objectCount, ObjectPool<TObject> pool, Func<TObject> factory, CancellationToken cancellationToken = default)
    {
        // Do not use Parallel.For here as not guaranteed to be thread safe
        for (int i = 0; i < objectCount; i++)
        {
            await pool._pool.Writer.WriteAsync(CreateObject(factory), cancellationToken).ConfigureAwait(false);
        }
    }

    private static TObject CreateObject(Func<TObject> factory)
    {
        TObject obj;
        try
        {
            obj = factory();
        }
        catch
        {
            throw;
        }

        return obj;
    }
}
