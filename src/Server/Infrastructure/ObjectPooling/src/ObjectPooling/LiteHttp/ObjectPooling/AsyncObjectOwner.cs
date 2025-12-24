using System.Diagnostics;

namespace LiteHttp.ObjectPooling;

public sealed class AsyncObjectOwner<Obj> : IAsyncDisposable where Obj : class
{
    private readonly Func<Obj, ValueTask> _returnCallback;
    private bool _returned;

    public Obj Object 
    {
        get 
        {
            Debug.Assert(!_returned, "Accessing returned object");
            if (!_returned)
            {
                return field;
            }
            throw new InvalidOperationException("Cannot access returned object");
        } 
    }

    public AsyncObjectOwner(Obj @object, Func<Obj, ValueTask> returnCallback)
    {
        Object = @object;
        _returnCallback = returnCallback;
        _returned = false;
    }

    public ValueTask DisposeAsync()
    {
        Debug.Assert(!_returned, "Object returned second time");
        
        if (!_returned)
        {
            _returned = true;
            return _returnCallback(Object);
        }
        
        throw new InvalidOperationException("Object cannot be returned to pool second time");
    }
}
