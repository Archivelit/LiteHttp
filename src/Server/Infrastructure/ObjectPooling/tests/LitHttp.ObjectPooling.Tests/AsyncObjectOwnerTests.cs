namespace LitHttp.ObjectPooling.Tests;

public class AsyncObjectOwnerTests
{
    [Fact]
    public async Task DisposeAsync_Should_InvokeCallback_And_MarkAsReturned()
    {
        // Arrange
        var wasCalled = false;
        var testObject = new object();
        ValueTask ReturnCallback(object obj)
        {
            wasCalled = true;
            return ValueTask.CompletedTask;
        }

        var owner = new AsyncObjectOwner<object>(testObject, ReturnCallback);
        
        // Act
        await owner.DisposeAsync();
        
        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task DisposeAsync_Should_ThrowInvalidOperationException_AfterSecondCall()
    {
        // Arrange
        var wasCalled = false;
        var testObject = new object();
        ValueTask ReturnCallback(object obj)
        {
            wasCalled = true;
            return ValueTask.CompletedTask;
        }

        var owner = new AsyncObjectOwner<object>(testObject, ReturnCallback);
        var disposeAsyncCallback = async () => owner.DisposeAsync();

        // Act
        await owner.DisposeAsync();

        // Assert
        wasCalled.Should().BeTrue();
        await disposeAsyncCallback.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public void GetObject_Should_RetrunObject_SeveralTimes()
    {
        // Arrange
        var testObject = new object();
        ValueTask ReturnCallback(object obj)
        {
            return ValueTask.CompletedTask;
        }

        var owner = new AsyncObjectOwner<object>(testObject, ReturnCallback);
        
        // Act
        var obj1 = owner.Object;
        var obj2 = owner.Object;
        var obj3 = owner.Object;

        // Assert
        ReferenceEquals(obj1, obj2).Should().BeTrue();
        ReferenceEquals(obj2, obj3).Should().BeTrue();
    }

    [Fact]
    public async Task GetObject_Should_ThrowInvalidOperationException_AfterDispose()
    {
        // Arrange
        var wasCalled = false;
        var testObject = new object();
        ValueTask ReturnCallback(object obj)
        {
            wasCalled = true;
            return ValueTask.CompletedTask;
        }

        var owner = new AsyncObjectOwner<object>(testObject, ReturnCallback);
        var accessObjectCallback = () => _ = owner.Object;

        // Act
        await owner.DisposeAsync();

        // Assert
        wasCalled.Should().BeTrue();
        accessObjectCallback.Should().Throw<InvalidOperationException>();
    }
}
