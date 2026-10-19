namespace CodeTest.Tests;

public class SubsequenceFinderTests
{
    [Theory]
    [MemberData(nameof(SubsequenceFinderTestData.TestCases), MemberType = typeof(SubsequenceFinderTestData))]
    public void ReturnsLongestIncreasingSubsequence(string testCaseName)
    {
        string input = SubsequenceFinderTestData.ReadInput(testCaseName);
        string expected = SubsequenceFinderTestData.ReadExpectedOutput(testCaseName);

        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1 2 3 1 2 3", "1 2 3")]
    [InlineData("5 6 1 2 3 4 7 8", "1 2 3 4 7 8")]
    public void ReturnsEarliestSubsequenceWhenTied(string input, string expected)
    {
        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1 2 2 3", "1 2")]
    [InlineData("1 1 1", "1")]
    public void EqualNumbersEndTheSubsequence(string input, string expected)
    {
        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("5 4 3 2 1", "5")]
    [InlineData("7", "7")]
    public void ReturnsFirstNumberWhenNothingIncreases(string input, string expected)
    {
        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Fact]
    public void HandlesNegativeNumbers()
    {
        Assert.Equal("-5 -2 0 3", SubsequenceFinder.LongestIncreasingSubsequence("1 -5 -2 0 3 -1"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ReturnsEmptyStringForEmptyInput(string input)
    {
        Assert.Equal("", SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Fact]
    public void ReturnsEmptyStringForNullInput()
    {
        Assert.Equal("", SubsequenceFinder.LongestIncreasingSubsequence(null!));
    }

    [Theory]
    [InlineData("6 x 5")]
    [InlineData("1.5 2")]
    public void ThrowsFormatExceptionForNonIntegerInput(string input)
    {
        Assert.Throws<FormatException>(() => SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Fact]
    public void ThrowsOverflowExceptionForNumbersOutsideIntRange()
    {
        Assert.Throws<OverflowException>(() => SubsequenceFinder.LongestIncreasingSubsequence("1 99999999999"));
    }
}
