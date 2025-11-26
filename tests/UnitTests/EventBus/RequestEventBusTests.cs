namespace UnitTests.EventBus;

public class RequestEventBusTests
{
    private readonly IEventBus<RequestReceivedEvent> _eventBus = new RequestEventBus();

    [Fact]
    public async Task ConsumeAsync_WhenChannelIsEmpty_ShouldWaitEventPublishment()
    {
        // Arrange
        var mockConnection = new Mock<Socket>(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var @event = new RequestReceivedEvent(mockConnection.Object);

        // Act
        var receivedEvent = Task.Run(() => _eventBus.ConsumeAsync(TestContext.Current.CancellationToken));
        await _eventBus.PublishAsync(@event, TestContext.Current.CancellationToken);

        // Assert
        receivedEvent.Should().NotBeNull();
        receivedEvent.Result.Result.Connection.Should().BeEquivalentTo(@event.Connection);
    }
}