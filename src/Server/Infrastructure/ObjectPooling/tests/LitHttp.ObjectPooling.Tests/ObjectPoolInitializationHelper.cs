namespace LiteHttp.ObjectPooling.Tests;

public class ObjectPoolInitializationHelperParameterizedTests
{
    public static IEnumerable<object[]> GetFactories()
    {
        var ct = CancellationToken.None;

        // 1. Sync factory
        yield return new object[] { new Func<TestObject>(() => new TestObject(1)) };

        // 2. Sync with CancellationToken
        yield return new object[] { new Func<CancellationToken, TestObject>(ct => new TestObject(2)) };

        // 3. Async Task factory
        yield return new object[] { new Func<Task<TestObject>>(async () => new TestObject(3)) };

        // 4. Async ValueTask factory
        yield return new object[] { new Func<ValueTask<TestObject>>(async () => new TestObject(4)) };

        // 5. Async with CancellationToken returning Task
        yield return new object[] { new Func<CancellationToken, Task<TestObject>>(async ct => new TestObject(5)) };

        // 6. Async with CancellationToken returning ValueTask
        yield return new object[] { new Func<CancellationToken, ValueTask<TestObject>>(async ct => new TestObject(6)) };
    }

    [Theory]
    [MemberData(nameof(GetFactories))]
    public async Task InitializeFactories_ShouldPopulatePoolCorrectly(object factoryObj)
    {
        var pool = new FakeObjectPool();

        switch (factoryObj)
        {
            case Func<TestObject> f:
                ObjectPoolInitializationHelper<TestObject>.Initialize(2, pool, f);
                break;
            case Func<CancellationToken, TestObject> f:
                await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, f);
                break;
            case Func<Task<TestObject>> f:
                await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, f);
                break;
            case Func<ValueTask<TestObject>> f:
                await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, f);
                break;
            case Func<CancellationToken, Task<TestObject>> f:
                await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, f);
                break;
            case Func<CancellationToken, ValueTask<TestObject>> f:
                await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, f);
                break;
            default:
                throw new NotSupportedException();
        }

        var count = pool.Channel.Reader.Count;
        count.Should().Be(2, "2 objects should be added to the pool");

        var r1 = pool.Channel.Reader.TryRead(out var obj1);
        var r2 = pool.Channel.Reader.TryRead(out var obj2);
        r1.Should().BeTrue();
        r2.Should().BeTrue();
        obj1.Should().NotBeNull();
        obj2.Should().NotBeNull();
        obj1.Should().BeEquivalentTo(obj2);
    }

    public static IEnumerable<object[]> GetFailingFactories()
    {
        var ct = CancellationToken.None;

        // 1. Sync factory
        yield return new object[] { new Func<TestObject>(() => throw new InvalidOperationException("fail")) };
            
        // 2. Sync with CancellationToken
        yield return new object[] { new Func<CancellationToken, TestObject>(_ => throw new InvalidOperationException("fail")) };

        // 3. Async Task factory
        yield return new object[] { new Func<Task<TestObject>>(async () => throw new InvalidOperationException("fail")) };

        // 4. Async ValueTask factory
        yield return new object[] { new Func<ValueTask<TestObject>>(async () => throw new InvalidOperationException("fail")) };

        // 5. Async with CancellationToken returning Task
        yield return new object[] { new Func<CancellationToken, Task<TestObject>>(async _ => throw new InvalidOperationException("fail")) };

        // 6. Async with CancellationToken returning ValueTask
        yield return new object[] { new Func<CancellationToken, ValueTask<TestObject>>(async _ => throw new InvalidOperationException("fail")) };
    }

    [Theory]
    [MemberData(nameof(GetFailingFactories))]
    public async Task InitializeAsync_FactoryThrows_ShouldThrow(object factoryObj)
    {
        var pool = new FakeObjectPool();

        switch (factoryObj)
        {
            case Func<TestObject> sf:
                Action syncAction = () => ObjectPoolInitializationHelper<TestObject>.Initialize(2, pool, sf);
                syncAction.Should().Throw<InvalidOperationException>();
                break;

            case Func<CancellationToken, TestObject> ctsf:
                Func<Task> ctSyncAction = async () => await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, ctsf);
                await ctSyncAction.Should().ThrowAsync<InvalidOperationException>();
                break;

            case Func<Task<TestObject>> tf:
                Func<Task> asyncAction = async () => await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, tf);
                await asyncAction.Should().ThrowAsync<InvalidOperationException>();
                break;

            case Func<ValueTask<TestObject>> vf:
                Func<Task> valueTaskAction = async () => await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, vf);
                await valueTaskAction.Should().ThrowAsync<InvalidOperationException>();
                break;

            case Func<CancellationToken, Task<TestObject>> cttf:
                Func<Task> ctTaskAction = async () => await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, cttf);
                await ctTaskAction.Should().ThrowAsync<InvalidOperationException>();
                break;

            case Func<CancellationToken, ValueTask<TestObject>> ctvf:
                Func<Task> ctValueTaskAction = async () => await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, ctvf);
                await ctValueTaskAction.Should().ThrowAsync<InvalidOperationException>();
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
