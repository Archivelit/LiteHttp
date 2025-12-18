namespace UnitTests.LiteHttp.Server;

public class ByteSpanComparerIgnoreCaseTests
{
    [Fact]
    public void Equals_SameSpan_ShouldReturn_True()
    {
        // Arrange
        var @string = "FooBuzz";
        var span1 = Encoding.ASCII.GetBytes(@string);
        var span2 = Encoding.ASCII.GetBytes(@string);

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_SameContent_DifferentCase_ShouldReturn_True()
    {
        // Arrange
        var span1 = Encoding.ASCII.GetBytes("foobuzz");
        var span2 = Encoding.ASCII.GetBytes("FOOBUZZ");

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Equals_SameContent_DifferentCase_WithSpecialCharacter_ShouldReturn_True()
    {
        // Arrange
        var span1 = Encoding.ASCII.GetBytes("foo-buzz");
        var span2 = Encoding.ASCII.GetBytes("FOO-BUZZ");

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Equals_SameSpan_WithSpecialCharacter_ShouldReturn_True()
    {
        // Arrange
        var @string = "Foo-Buzz";
        var span1 = Encoding.ASCII.GetBytes(@string);
        var span2 = Encoding.ASCII.GetBytes(@string);

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_SameContent_SpecialCharactersOnly_ShouldReturn_True()
    {
        // Arrange
        var @string = "!@#$%^&*()_+=";
        var span1 = Encoding.ASCII.GetBytes(@string);
        var span2 = Encoding.ASCII.GetBytes(@string);

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Equals_DifferentContent_SpecialCharactersOnly_ShouldReturn_False()
    {
        // Arrange
        var span1 = Encoding.ASCII.GetBytes("!@#");
        var span2 = Encoding.ASCII.GetBytes("$%^");

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentContent_ShouldReturn_False()
    {
        // Arrange
        var span1 = Encoding.ASCII.GetBytes("FooBuzz");
        var span2 = Encoding.ASCII.GetBytes("BuzzFoo");

        // Act
        var result = ByteSpanComparerIgnoreCase.Equals(span1, span2);
        
        // Assert
        result.Should().BeFalse();
    }
}