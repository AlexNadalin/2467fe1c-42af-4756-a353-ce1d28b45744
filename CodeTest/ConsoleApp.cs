namespace CodeTest;

public static class ConsoleApp
{
    public const int SuccessExitCode = 0;
    public const int InvalidInputExitCode = 1;

    /// <summary>
    /// Reads one line of whitespace-separated integers and writes the longest increasing subsequence.
    /// </summary>
    /// <remarks>
    /// Only the result is written to <paramref name="output"/>. The prompt and any error messages are
    /// written to <paramref name="error"/>, so scripts and containers can read the result from standard
    /// output and detect failures from the exit code.
    /// </remarks>
    /// <param name="input">Where the numbers are read from, e.g. <see cref="Console.In"/>.</param>
    /// <param name="output">Where the result is written, e.g. <see cref="Console.Out"/>.</param>
    /// <param name="error">Where the prompt and error messages are written, e.g. <see cref="Console.Error"/>.</param>
    /// <returns><see cref="SuccessExitCode"/> on success, or <see cref="InvalidInputExitCode"/> if the input is invalid.</returns>
    public static int Run(TextReader input, TextWriter output, TextWriter error)
    {
        error.Write("Enter numbers separated by spaces: ");
        string line = input.ReadLine() ?? "";

        try
        {
            output.WriteLine(SubsequenceFinder.LongestIncreasingSubsequence(line));
            return SuccessExitCode;
        }
        catch (FormatException)
        {
            error.WriteLine("Invalid input: please enter whole numbers separated by spaces, e.g. 6 1 5 9 2");
            return InvalidInputExitCode;
        }
        catch (OverflowException)
        {
            error.WriteLine($"Invalid input: numbers must be between {int.MinValue} and {int.MaxValue}.");
            return InvalidInputExitCode;
        }
    }
}
