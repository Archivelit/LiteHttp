namespace LiteHttp.ObjectPooling;

public abstract class ObjectPool<TObject> where TObject : class
{
    protected internal readonly Func<TObject> _objectGenerator;
    protected internal readonly Channel<TObject> _pool;

    public ObjectPool(Func<TObject> objectGenerator)
    {
        ArgumentNullException.ThrowIfNull(objectGenerator);

        _objectGenerator = objectGenerator;
        _pool = Channel.CreateUnbounded<TObject>();
    }

    protected internal virtual TObject InternalGetOrGenerate()
    {
        if(InternalTryGet(out var obj))
            return obj;
        
        return _objectGenerator();
    }

    protected internal bool InternalTryGet([NotNullWhen(true)] out TObject? obj) => 
        _pool.Reader.TryRead(out obj);

    protected internal bool InternalTryReturn(TObject obj) => 
        _pool.Writer.TryWrite(obj);

    protected internal ValueTask<TObject> InternalGetAsync() => _pool.Reader.ReadAsync();

    protected internal ValueTask InternalReturnAsync(TObject obj) => _pool.Writer.WriteAsync(obj);
}