namespace LiteHttp.ObjectPooling.Tests;

public class AsyncOwnedObjectPoolTests
{
    [Fact]
    public void TryGet_ObjectAvailable_ReturnsObjectOwner()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        var obj = new TestObject(2);
        
        ObjectPoolInitializationHelper<TestObject>.Initialize(1, pool, () => obj);
        // Act
        var result = pool.TryGet(out var objectOwner);

        // Assert
        result.Should().BeTrue();
        objectOwner.Should().NotBeNull();
        objectOwner.Object.Should().BeSameAs(obj);
    }

    [Fact]
    public void TryGet_NoObjectAvailable_ReturnsFalse()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        
        // Act
        var result = pool.TryGet(out var objectOwner);
     
        // Assert
        result.Should().BeFalse();
        objectOwner.Should().BeNull();
    }

    [Fact]
    public async Task TryGet_ObjectReturned_CanBeRetrievedAgain()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        var obj = new TestObject(3);
        
        ObjectPoolInitializationHelper<TestObject>.Initialize(1, pool, () => obj);
        
        // Act
        pool.TryGet(out var objectOwner).Should().BeTrue();

        await objectOwner!.DisposeAsync();

        var result = pool.TryGet(out var secondOwner);

        // Assert
        result.Should().BeTrue();
        secondOwner.Should().NotBeNull();
        secondOwner!.Object.Should().BeSameAs(obj);
    }

    [Fact]
    public async Task GetAsync_ObjectAvailable_ReturnsObjectOwner()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        var obj = new TestObject(4);
        var ct = TestContext.Current.CancellationToken;

        ObjectPoolInitializationHelper<TestObject>.Initialize(1, pool, () => obj);
        
        // Act
        var objectOwner = await pool.GetAsync(ct);
        
        // Assert
        objectOwner.Should().NotBeNull();
        objectOwner.Object.Should().BeSameAs(obj);
    }

    [Fact]
    public async Task GetAsync_NoObjectAvailable_WaitsAndReturnsObjectOwner()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        var obj1 = new TestObject(5);
        var ct = TestContext.Current.CancellationToken;

        async Task AddingTask() => ObjectPoolInitializationHelper<TestObject>.Initialize(1, pool, () => obj1);

        // Act
        var getTask = Task.Run(async () => await pool.GetAsync(ct));
        await Task.WhenAll(getTask, Task.Run(AddingTask, ct));
        
        var secondOwner = await getTask;

        // Assert
        secondOwner.Should().NotBeNull();
        secondOwner.Object.Should().BeSameAs(obj1);
    }

    [Fact]
    public async Task GetAsync_ObjectReturned_CanBeRetrievedAgain()
    {
        // Arrange
        var pool = new AsyncOwnedObjectPool<TestObject>();
        var obj = new TestObject(6);
        var ct = TestContext.Current.CancellationToken;
        
        ObjectPoolInitializationHelper<TestObject>.Initialize(1, pool, () => obj);
        
        // Act
        var objectOwner = await pool.GetAsync(ct);
        
        await objectOwner.DisposeAsync();
        
        var secondOwner = await pool.GetAsync(ct);
        
        // Assert
        secondOwner.Should().NotBeNull();
        secondOwner.Object.Should().BeSameAs(obj);
    }

    [Fact]
    public async Task GetAsync_TwoWaiters_OnlyOneReceivesObject()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var testObject = new TestObject(1);
        var ct = TestContext.Current.CancellationToken;

        var started1 = new TaskCompletionSource();
        var started2 = new TaskCompletionSource();

        async Task<TestObject> Waiter(TaskCompletionSource started)
        {
            started.SetResult();
            return await pool.GetAsync(ct);
        }

        // Act
        var t1 = Task.Run(() => Waiter(started1), ct);
        var t2 = Task.Run(() => Waiter(started2), ct);
        var delayTask = Task.Delay(TimeSpan.FromSeconds(5), ct);

        await Task.WhenAll(started1.Task, started2.Task);

        await pool.ReturnAsync(testObject, ct);

        var completed = await Task.WhenAny(t1, t2, delayTask);

        // Assert
        if (ReferenceEquals(completed, delayTask))
        {
            completed.Should().NotBeSameAs(delayTask, "One of the waiters should have completed before the timeout");
            return;
        }

        Task<TestObject> completedTask = (Task<TestObject>)completed;
        var result = await completedTask;
        result.Should().BeSameAs(testObject);

        var pending = completed == t1 ? t2 : t1;
        pending.IsCompleted.Should().BeFalse();
    }
}
