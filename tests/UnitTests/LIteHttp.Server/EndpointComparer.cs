namespace LiteHttp.Benchmarks;

public class EndpointComparerTests
{
    private IEqualityComparer<Endpoint> _comparer = new EndpointComparer();

    [Fact]
    public void CheckEndpointEquality_DifferentEndpoints_ShouldReturnFalse()
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
    public void CheckEndpointEquality_DifferentLinks_ShouldReturTrue()
    {
        // Arrange
        var endpoint1 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);
        var endpoint2 = new Endpoint(Encoding.UTF8.GetBytes("/api/test"), RequestMethodsAsBytes.Post);

        // Act
        var result = _comparer.Equals(endpoint1, endpoint2);

        // Assert
        result.Should().BeTrue();
    }
}