using System.Net.NetworkInformation;

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


    [Fact]
    public void WithAddress_ValidIPv6Address_ShouldThrow_NotSupportedException()
    {
        // Arrange
        var address = IPAddress.IPv6Loopback;
        var builder = new ServerBuilder();

        var action = () => builder.WithAddress(address);

        // Act & Assert
        action.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void WithAddress_IPv4AddressWithChars_ShouldThrow_FormatException()
    {
        // Arrange
        var address = "123.123.123.asd";
        var builder = new ServerBuilder();

        var action = () => builder.WithAddress(address);

        // Act & Assert
        action.Should().Throw<FormatException>();
    }

    
    [Fact]
    public void WithAddress_TooLongIPv4Address_ShouldThrow_FormatException()
    {
        // Arrange
        var address = "123.123.123.1234";
        var builder = new ServerBuilder();

        var action = () => builder.WithAddress(address);

        // Act & Assert
        action.Should().Throw<FormatException>();
    }

        
    [Fact]
    public void WithAddress_NullParameter_ShouldThrow_ArgumentNullException()
    {
        // Arrange
        string? address = null;
        var builder = new ServerBuilder();

        var action = () => builder.WithAddress(address!);

        // Act & Assert
        action.Should().Throw<ArgumentNullException>();
    }
    
    
    [Fact]
    public void WithAddress_IPv4Address_ShouldSet()
    {
        // Arrange
        var address = IPAddress.Loopback;
        var builder = new ServerBuilder();
        var field = GetIpAddressField(builder);

        var action = () => builder.WithAddress(address);

        // Act & Assert
        action.Should().NotThrow();
    }

    private static System.Reflection.FieldInfo? GetIpAddressField(ServerBuilder builder) =>
        builder.GetType().GetField("_address");
}