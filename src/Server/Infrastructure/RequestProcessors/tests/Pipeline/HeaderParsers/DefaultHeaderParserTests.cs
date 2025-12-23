using LiteHttp.RequestProcessors.PipeContext.Parser;

using Xunit.v3;

namespace LiteHttp.RequestProcessors.Tests.PipeContext.Parser;

public class DefaultHeaderParserTests
{
    private readonly DefaultHeaderParser _parser = new();
    private readonly ITestOutputHelper _outputHelper = TestContext.Current.TestOutputHelper ?? new TestOutputHelper();

    [Fact]
    public void ParseHeader_WithValidHeader_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host: test.com\r\n".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        WriteErrorIfReturned(result);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }

    [Fact]
    public void ParseHeader_WithValidHeader_WithoutLF_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host: test.com\r".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        WriteErrorIfReturned(result);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }

    [Fact]
    public void ParseHeader_WithValidHeader_WithoutSpaceInHeaderSeparator_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host:test.com\r".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        WriteErrorIfReturned(result);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }

    [Fact]
    public void ParseHeader_WithInvalidHeader_WithoutValue_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host:\r".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(HeaderParsingErrors.HeaderSyntaxError);
    }

    [Fact]
    public void ParseHeader_WithInvalidHeaders_TwoSameHeaders_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host:Foo\r".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        _parser.ParseHeader(headerLineSequence, headerCollection);
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(HeaderParsingErrors.TwoSameHeaders);
    }

    [Fact]
    public void ParseHeader_WithValidHeader_WithoutCR_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host: test.com".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        WriteErrorIfReturned(result);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }

    [Fact]
    public void ParseHeader_WithValidHeader_WithoutCR_WithLF_ReturnsSuccessful()
    {
        // Arrange
        var headerLine = "Host: test.com\n".AsMemoryByteArray();
        var headerCollection = new HeaderCollection();
        var headerLineSequence = new ReadOnlySequence<byte>(headerLine);

        // Act
        var result = _parser.ParseHeader(headerLineSequence, headerCollection);

        // Assert
        WriteErrorIfReturned(result);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }

#nullable disable
    private void WriteErrorIfReturned(HeaderParsingResult result)
    {
        if (!result.Success)
        {
            _outputHelper.WriteLine(result.Error.Value.ErrorMessage);
        }
    }
}
