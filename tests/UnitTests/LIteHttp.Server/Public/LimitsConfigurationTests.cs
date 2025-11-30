namespace UnitTests.LiteHttp.Server.Public;

public class LimitsConfigurationTests
{
    #region KeepAliveTimeout tests
    [Fact]
    public void KeepAliveTimeout_TimeSpanZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.KeepAliveTimeout = TimeSpan.Zero;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void KeepAliveTimeout_NegativeTimeSpanAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.KeepAliveTimeout = TimeSpan.FromSeconds(-15);
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void KeepAliveTimeout_InfiniteTimeSpanAssigned_Should_Change()
    {
        // Arrange
        var expected = TimeSpan.MaxValue;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.KeepAliveTimeout = Timeout.InfiniteTimeSpan;
        });

        // Assert
        configuration.KeepAliveTimeout.Should().Be(expected);
    }

    [Fact]
    public void KeepAliveTimeout_DefaultTimeoutAssigned_Should_Change()
    {
        // Arrange
        var expected = TimeSpan.FromSeconds(40);

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.KeepAliveTimeout = expected;
        });

        // Assert
        configuration.KeepAliveTimeout.Should().Be(expected);
    }
    #endregion

    #region RequestHeadersTimeout tests
    [Fact]
    public void RequestHeadersTimeout_TimeSpanZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.RequestHeadersTimeout = TimeSpan.Zero;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RequestHeadersTimeout_NegativeTimeSpanAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.RequestHeadersTimeout = TimeSpan.FromSeconds(-15);
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RequestHeadersTimeout_InfiniteTimeSpanAssigned_Should_Change()
    {
        // Arrange
        var expected = TimeSpan.MaxValue;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.RequestHeadersTimeout = Timeout.InfiniteTimeSpan;
        });

        // Assert
        configuration.RequestHeadersTimeout.Should().Be(expected);
    }

    [Fact]
    public void RequestHeadersTimeout_DefaultTimeoutAssigned_Should_Change()
    {
        // Arrange
        var expected = TimeSpan.FromSeconds(40);

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.RequestHeadersTimeout = expected;
        });

        // Assert
        configuration.RequestHeadersTimeout.Should().Be(expected);
    }
    #endregion

    #region MaxConcurrentConnections tests
    [Fact]
    public void MaxConcurrentConnections_ZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxConcurrentConnections_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxConcurrentConnections_NullAssigned_Should_BeNull()
    {
        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = null;
        });

        // Assert
        configuration.MaxConcurrentConnections.Should().BeNull();
    }

    [Fact]
    public void MaxConcurrentConnections_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = expected;
        });

        // Assert
        configuration.MaxConcurrentConnections.Should().Be(expected);
    }
    #endregion

    #region MaxConcurrentUpgradedConnections tests
    [Fact]
    public void MaxConcurrentUpgradedConnections_ZeroAssigned_Should_Change()
    {
        // Assert
        var expected = 0;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxConcurrentUpgradedConnections = expected;
        });

        // Assert
        configuration.MaxConcurrentUpgradedConnections.Should().Be(expected);
    }

    [Fact]
    public void MaxConcurrentUpgradedConnections_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxConcurrentUpgradedConnections_NullAssigned_Should_BeNull()
    {
        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = null;
        });

        // Assert
        configuration.MaxConcurrentConnections.Should().BeNull();
    }

    [Fact]
    public void MaxConcurrentUpgradedConnections_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxConcurrentConnections = expected;
        });

        // Assert
        configuration.MaxConcurrentConnections.Should().Be(expected);
    }
    #endregion

    #region MaxRequestHeaderCount tests
    [Fact]
    public void MaxRequestHeaderCount_ZeroAssigned_Should_Throw_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestHeaderCount = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestHeaderCount_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestHeaderCount = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestHeaderCount_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestHeaderCount = expected;
        });

        // Assert
        configuration.MaxRequestHeaderCount.Should().Be(expected);
    }

    [Fact]
    public void MaxRequestHeaderCount_DefaultValueAssigned_ShouldNot_Change()
    {
        // Act
        var configuration1 = new LimitsConfiguration(o => { });
        var configuration2 = new LimitsConfiguration(options =>
        {
            options.MaxRequestHeaderCount = configuration1.MaxRequestHeaderCount;
        });

        // Assert
        configuration1.MaxRequestHeaderCount.Should().Be(configuration2.MaxRequestHeaderCount);
    }
    #endregion

    #region MaxResponseBufferSize tests
    [Fact]
    public void MaxResponseBufferSize_ZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxResponseBufferSize = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxResponseBufferSize_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxResponseBufferSize = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxResponseBufferSize_NullAssigned_Should_BeNull()
    {
        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxResponseBufferSize = null;
        });

        // Assert
        configuration.MaxResponseBufferSize.Should().BeNull();
    }

    [Fact]
    public void MaxResponseBufferSize_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxResponseBufferSize = expected;
        });

        // Assert
        configuration.MaxResponseBufferSize.Should().Be(expected);
    }
    #endregion

    #region MaxRequestBufferSize tests
    [Fact]
    public void MaxRequestBufferSize_ZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestBufferSize = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestBufferSize_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestBufferSize = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestBufferSize_NullAssigned_Should_BeNull()
    {
        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestBufferSize = null;
        });

        // Assert
        configuration.MaxRequestBufferSize.Should().BeNull();
    }

    [Fact]
    public void MaxRequestBufferSize_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestBufferSize = expected;
        });

        // Assert
        configuration.MaxRequestBufferSize.Should().Be(expected);
    }
    #endregion

    #region MaxRequestLineSize tests
    [Fact]
    public void MaxRequestLineSize_ZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestLineSize = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestLineSize_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestLineSize = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestLineSize_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestLineSize = expected;
        });

        // Assert
        configuration.MaxRequestLineSize.Should().Be(expected);
    }
    #endregion

    #region MaxRequestBodySize tests
    [Fact]
    public void MaxRequestBodySize_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestBodySize = -1;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestBodySize_NullAssigned_Should_BeNull()
    {
        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestBodySize = null;
        });

        // Assert
        configuration.MaxRequestBodySize.Should().BeNull();
    }

    [Fact]
    public void MaxRequestBodySize_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 123;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestBodySize = expected;
        });

        // Assert
        configuration.MaxRequestBodySize.Should().Be(expected);
    }
    #endregion

    #region MaxRequestHeadersTotalSize tests
    [Fact]
    public void MaxRequestHeadersTotalSize_ZeroAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestHeadersTotalSize = 0;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestHeadersTotalSize_NegativeNumberAssigned_ShouldThrow_ArgumentOutOfRangeException()
    {
        // Act
        var action = () => new LimitsConfiguration(options =>
        {
            options.MaxRequestHeadersTotalSize = -15;
        });

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxRequestHeadersTotalSize_PositiveNumberAssigned_Should_Change()
    {
        // Arrange
        var expected = 30;

        // Act
        var configuration = new LimitsConfiguration(options =>
        {
            options.MaxRequestHeadersTotalSize = expected;
        });

        // Assert
        configuration.MaxRequestHeadersTotalSize.Should().Be(expected);
    }
    #endregion
}