using LiteHttp;

namespace UnitTests.LiteHttp.Server.Public;

public class ServerBuilderTests
{
    [Fact]
    public void Build_InvalidPort_ShouldThrow_ArgumentException()
    {
        // Arrange
        var port = -123;
        var address = IPAddress.Loopback;
        var logger = new NullLogger();

        var builder = new ServerBuilder();

        var action = () => builder
            .WithAddress(address)
            .WithPort(port)
            .WithLogger(logger)
            .Build();

        // Act & Assert
        action.Should().Throw<ArgumentException>();
    }
}