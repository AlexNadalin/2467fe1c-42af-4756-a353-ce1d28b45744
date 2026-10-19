namespace CodeTest.Tests;

public class ConsoleAppTests
{
    [Fact]
    public void WritesOnlyTheResultToOutputForValidInput()
    {
        var (exitCode, output, _) = RunWithInput("6 1 5 9 2");

        Assert.Equal(ConsoleApp.SuccessExitCode, exitCode);
        Assert.Equal("1 5 9" + Environment.NewLine, output);
    }

    [Fact]
    public void WritesThePromptToError()
    {
        var (_, _, error) = RunWithInput("6 1 5 9 2");

        Assert.Equal("Enter numbers separated by spaces: ", error);
    }

    [Theory]
    [InlineData("1 x 3", "please enter whole numbers")]
    [InlineData("1 99999999999", "numbers must be between")]
    public void WritesErrorAndReturnsFailureExitCodeForInvalidInput(string input, string expectedMessage)
    {
        var (exitCode, output, error) = RunWithInput(input);

        Assert.Equal(ConsoleApp.InvalidInputExitCode, exitCode);
        Assert.Equal("", output);
        Assert.Contains(expectedMessage, error);
    }

    [Fact]
    public void ReturnsSuccessWithEmptyResultWhenNoInputIsGiven()
    {
        // An empty reader behaves like the end of input, so ReadLine returns null
        var (exitCode, output, _) = RunWithInput("");

        Assert.Equal(ConsoleApp.SuccessExitCode, exitCode);
        Assert.Equal(Environment.NewLine, output);
    }

    private static (int ExitCode, string Output, string Error) RunWithInput(string input)
    {
        using var reader = new StringReader(input);
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = ConsoleApp.Run(reader, output, error);

        return (exitCode, output.ToString(), error.ToString());
    }
}
