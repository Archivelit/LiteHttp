
namespace LiteHttp.ObjectPooling.Tests;

internal class FakeObjectPool : ObjectPool<TestObject>
{
    public FakeObjectPool(Func<TestObject> objectGenerator) : base(objectGenerator)
    { }
}
