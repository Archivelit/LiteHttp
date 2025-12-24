namespace LiteHttp.ObjectPooling.Tests;

public class ObjectPoolInitializationHelperTests
{
    private readonly ITestOutputHelper _testOutputHelper = TestContext.Current.TestOutputHelper ?? new TestOutputHelper();

    [Fact]
    public void Initialize_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var pool = new DefaultObjectPool<TestObject>(() => new TestObject(0));

        ObjectPoolInitializationHelper<TestObject>.Initalize(2, pool, () => new TestObject(2));

        // Act
        var obj1 = pool.GetOrGenerate();
        var obj2 = pool.GetOrGenerate();

        // Assert
        obj1.Should().BeEquivalentTo(obj2);
        obj1.Id.Should().Be(2);
    }
}
