namespace LiteHttp.ObjectPooling.Tests;

public class DefaultObjectPoolTests
{
    [Fact]
    public void TryGet_Should_ReturnObject_When_Available()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var testObject = new TestObject(1);
        pool.TryReturn(testObject);

        // Act
        var result = pool.TryGet(out var obj);
        
        // Assert
        result.Should().BeTrue();
        obj.Should().BeEquivalentTo(testObject);
    }

    [Fact]
    public void TryGet_Should_ReturnFalse_When_Empty()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();

        // Act
        var result = pool.TryGet(out var obj);
        
        // Assert
        result.Should().BeFalse();
        obj.Should().BeNull();
    }

    [Fact]
    public void TryReturn_Should_ReturnTrue_When_ObjectReturned()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var testObject = new TestObject(1);
        
        // Act
        var result = pool.TryReturn(testObject);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void TryReturn_Should_Handle_Returning_NonPooledObject()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var externalObject = new TestObject(2);
        
        // Act
        var result = pool.TryReturn(externalObject);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnAsync_Should_ReturnObject()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var testObject = new TestObject(1);

        // Act
        await pool.ReturnAsync(testObject);
        var result = pool.TryGet(out var obj);

        // Assert
        result.Should().BeTrue();
        obj.Should().NotBeNull();
        obj.Should().BeEquivalentTo(testObject);
    }

    [Fact]
    public async Task GetAsync_Should_Wait_And_ReturnObject_When_Available()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();
        var testObject = new TestObject(1);
        var ct = TestContext.Current.CancellationToken;

        // Act
        var res = Task.Run(async () => await pool.GetAsync(ct), ct);
        
        await pool.ReturnAsync(testObject, ct);
        
        // Assert
        res.Result.Should().BeEquivalentTo(testObject);
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
