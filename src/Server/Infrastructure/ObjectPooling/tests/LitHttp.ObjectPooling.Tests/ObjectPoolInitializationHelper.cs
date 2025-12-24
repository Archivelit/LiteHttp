namespace LiteHttp.ObjectPooling.Tests;

public class ObjectPoolInitializationHelperTests
{
    private readonly ITestOutputHelper _testOutputHelper = TestContext.Current.TestOutputHelper ?? new TestOutputHelper();

    [Fact]
    public void Initialize_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>();

        ObjectPoolInitializationHelper<TestObject>.Initalize(2, pool, () => new TestObject(2));

        // Act
        var r1 = pool.TryGet(out var obj1);
        var r2 = pool.TryGet(out var obj2);

        // Assert
        r1.Should().BeTrue();
        r2.Should().BeFalse();

        obj1!.Id.Should().Be(2);
        obj1.Should().BeEquivalentTo(obj2);
    }
}
