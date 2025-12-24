namespace LiteHttp.ObjectPooling.Tests;

public class ObjectPoolInitializationHelperTests
{
    private readonly ITestOutputHelper _testOutputHelper = TestContext.Current.TestOutputHelper ?? new TestOutputHelper();

    [Fact]
    public void Initialize_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var pool = new FakeObjectPool();


        // Act
        ObjectPoolInitializationHelper<TestObject>.Initalize(2, pool, () => new TestObject(2));
        
        var count = pool.Channel.Reader.Count;
        
        var r1 = pool.Channel.Reader.TryRead(out var obj1);
        var r2 = pool.Channel.Reader.TryRead(out var obj2);

        // Assert
        count.Should().Be(2);
        r1.Should().BeTrue();
        r2.Should().BeTrue();

        obj1!.Id.Should().Be(2);
        obj1.Should().BeEquivalentTo(obj2);
    }

    [Fact]
    public async Task InitializeAsync_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var pool = new FakeObjectPool();


        // Act
        await ObjectPoolInitializationHelper<TestObject>.InitializeAsync(2, pool, () => new TestObject(2), 
            TestContext.Current.CancellationToken);
        
        var count = pool.Channel.Reader.Count;

        var r1 = pool.Channel.Reader.TryRead(out var obj1);
        var r2 = pool.Channel.Reader.TryRead(out var obj2);

        // Assert
        count.Should().Be(2);
        r1.Should().BeTrue();
        r2.Should().BeTrue();

        obj1!.Id.Should().Be(2);
        obj1.Should().BeEquivalentTo(obj2);
    }
}
