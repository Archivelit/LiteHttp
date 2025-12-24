namespace LiteHttp.ObjectPooling.Tests;

internal class FakeObjectPool : ObjectPool<TestObject>
{
    public FakeObjectPool() : base() { }

    public Channel<TestObject> Channel => _pool;
}
