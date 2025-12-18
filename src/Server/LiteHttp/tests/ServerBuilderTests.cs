using LiteHttp;

namespace UnitTests.LiteHttp.Server.Public;

public class ServerBuilderTests
{
    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    public void Build_InvalidWorkersCound_ShouldThrow_ArgumentException(int workersCount)
    {
        // Arrange
        var port = 8080;
        var address = IPAddress.Loopback;
        var logger = new NullLogger();

        var builder = new ServerBuilder();

        var action = () => builder
            .WithAddress(address)
            .WithPort(port)
            .WithWorkersCount(workersCount)
            .WithLogger(logger)
            .Build();

        // Act & Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Build_InvalidPort_ShouldThrow_ArgumentException()
    {
        // Arrange
        var port = -123;
        var workersCount = 4;
        var address = IPAddress.Loopback;
        var logger = new NullLogger();

        var builder = new ServerBuilder();

        var action = () => builder
            .WithAddress(address)
            .WithPort(port)
            .WithWorkersCount(workersCount)
            .WithLogger(logger)
            .Build();

        // Act & Assert
        action.Should().Throw<ArgumentException>();
    }
}