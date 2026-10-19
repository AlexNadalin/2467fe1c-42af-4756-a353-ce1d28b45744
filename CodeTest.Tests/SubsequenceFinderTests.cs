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

    // Each case has tied runs with different values, so returning a later run would fail the test
    [Theory]
    [InlineData("1 2 3 0 4 5", "1 2 3")]
    [InlineData("5 9 1 4 2 3", "5 9")]
    public void ReturnsEarliestSubsequenceWhenTied(string input, string expected)
    {
        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("5 6 1 2 3 4 7 8", "1 2 3 4 7 8")]
    [InlineData("1 2 0 1 2 3", "0 1 2 3")]
    public void ReturnsLaterSubsequenceWhenItIsLonger(string input, string expected)
    {
        Assert.Equal(expected, SubsequenceFinder.LongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1\t2\t3", "1 2 3")]
    [InlineData("6\t1 5\n9  2", "1 5 9")]
    [InlineData("\r\n6 1 5 9 2\r\n", "1 5 9")]
    public void AcceptsAnyWhitespaceAsSeparator(string input, string expected)
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
    [InlineData("\t")]
    [InlineData(" \t\r\n ")]
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
