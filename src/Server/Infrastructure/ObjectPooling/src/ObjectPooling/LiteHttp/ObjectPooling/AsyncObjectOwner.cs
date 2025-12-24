namespace LiteHttp.ObjectPooling;

/// <summary>
/// Wrapper for an object owned from an <see cref="AsyncOwnedObjectPool{Obj}"/>.
/// </summary>
public struct AsyncObjectOwner<Obj> : IAsyncDisposable where Obj : class
{
    /// <summary>
    /// Callback to return the object to the pool.
    /// </summary>
    private readonly Func<Obj, ValueTask> _returnCallback;
    /// <summary>
    /// Determines whether the object has been returned.
    /// </summary>
    private bool _returned;

    /// <summary>
    /// An object owned from the pool.
    /// </summary>
    /// <remarks>
    /// Cannot be accessed after the object has been returned to the pool.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if object has been accessed after returning to the pool.
    /// </exception>
    public readonly Obj Object 
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

    /// <summary>
    /// Initializes a new instance of the AsyncObjectOwner class with the specified object and return callback.
    /// </summary>
    /// <remarks>
    /// Not intended for use outside of the object pooling infrastructure.
    /// </remarks>
    /// <param name="object">The object to be owned and managed by this instance. Cannot be null.</param>
    /// <param name="returnCallback">A callback function that is invoked asynchronously to return the object when ownership ends. Cannot be null.</param>
    public AsyncObjectOwner(Obj @object, Func<Obj, ValueTask> returnCallback)
    {
        Object = @object;
        _returnCallback = returnCallback;
        _returned = false;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>ValueTask callback that returns the owned object to the pool asynchronously.</returns>
    /// <exception cref="InvalidOperationException">Thrown when method is called two times.</exception>
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
