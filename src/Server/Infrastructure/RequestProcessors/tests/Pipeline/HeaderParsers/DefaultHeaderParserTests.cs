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
        if (!result.Success)
        {
            _outputHelper.WriteLine("Parsing failed with error: " + result.Error);
        }

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();

        headerCollection.Headers.TryGetValue("Host".AsMemoryByteArray(), out var value).Should().BeTrue();
        Encoding.ASCII.GetString(value.ToArray()).Should().BeEquivalentTo("test.com");
    }
}
