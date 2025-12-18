namespace UnitTests.LiteHttp.Server;

public class EndpointComparerTests
{
    private readonly EndpointComparer _comparer = new EndpointComparer();

    [Fact]
    public void CheckEndpointEquality_DifferentMethods_ShouldReturnFalse()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Get);

        // Act
        var result = _comparer.Equals(endpoint1, endpoint2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CheckEndpointEquality_SameEndpoints_ShouldReturTrue()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);

        // Act
        var result = _comparer.Equals(endpoint1, endpoint2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CheckEndpointEquality_DifferentEndpointCharCase_ShouldReturnFalse()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/API/TEST"), RequestMethodsAsBytes.Post);

        // Act
        var result = _comparer.Equals(endpoint1, endpoint2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CheckEndpointEquality_DifferentRoutes_ShouldReturnFalse()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/users"), RequestMethodsAsBytes.Post);

        // Act
        var result = _comparer.Equals(endpoint1, endpoint2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameEndpoints_ShouldReturn_SameHash()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);

        // Act
        var hash1 = _comparer.GetHashCode(endpoint1);
        var hash2 = _comparer.GetHashCode(endpoint2);

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void GetHashCode_DifferentRouteSequence_ShouldReturn_DifferentHash()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/test/api"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);

        // Act
        var hash1 = _comparer.GetHashCode(endpoint1);
        var hash2 = _comparer.GetHashCode(endpoint2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void GetHashCode_DifferentMethods_ShouldReturn_DifferentHash()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Get);

        // Act
        var hash1 = _comparer.GetHashCode(endpoint1);
        var hash2 = _comparer.GetHashCode(endpoint2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void GetHashCode_EmptyRoute_ShouldNotThrow()
    {
        // Arrange
        var endpoint = new Endpoint(Encoding.UTF8.GetBytes(string.Empty), RequestMethodsAsBytes.Post);
        var action = () => _comparer.GetHashCode(endpoint);

        // Act & Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void GetHashCode_NullRoute_ShouldNotThrow()
    {
        // Arrange
        var endpoint = new Endpoint(null, RequestMethodsAsBytes.Post);
        var action = () => _comparer.GetHashCode(endpoint);

        // Act & Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void GetHashCode_EmptyMethod_ShouldNotThrow()
    {
        // Arrange
        var endpoint = new Endpoint(Encoding.UTF8.GetBytes("/api/test"),
            Encoding.UTF8.GetBytes(string.Empty));

        var action = () => _comparer.GetHashCode(endpoint);

        // Act & Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void GetHashCode_NullMethod_ShouldNotThrow()
    {
        // Arrange
        var endpoint = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), null);
        var action = () => _comparer.GetHashCode(endpoint);

        // Act & Assert
        var result = action.Should().NotThrow();
    }
}