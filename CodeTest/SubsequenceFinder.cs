using System.Globalization;

namespace CodeTest;

public static class SubsequenceFinder
{
    /// <summary>
    /// Finds the longest run of strictly increasing adjacent integers in a whitespace-separated string.
    /// </summary>
    /// <remarks>
    /// Scans the numbers once from left to right, tracking where the current increasing run
    /// started. Whenever a number is not greater than the one before it, a new run begins at
    /// that number. The longest run seen so far is kept, and it is only replaced by a strictly
    /// longer run, so when several runs share the longest length the earliest one is returned.
    /// Equal adjacent numbers do not count as increasing.
    /// <para>
    /// Example: for "6 1 5 9 2" the runs are "6", "1 5 9" and "2", so the result is "1 5 9".
    /// </para>
    /// </remarks>
    /// <param name="input">Integers separated by whitespace, e.g. "6 1 5 9 2".</param>
    /// <returns>
    /// The longest increasing run as a space-separated string, or an empty string if
    /// <paramref name="input"/> is null, empty or contains only whitespace.
    /// </returns>
    /// <exception cref="FormatException">Thrown if any value in <paramref name="input"/> is not an integer.</exception>
    /// <exception cref="OverflowException">Thrown if any value in <paramref name="input"/> is outside the range of an <see cref="int"/>.</exception>
    public static string LongestIncreasingSubsequence(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        // A null separator splits on any whitespace character (spaces, tabs, new lines)
        int[] numbers = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .Select(number => int.Parse(number, CultureInfo.InvariantCulture))
            .ToArray();

        int currentStartIndex = 0;
        int bestStartIndex = 0;
        int bestSubsequenceLength = 1;

        for (int i = 1; i < numbers.Length; i++)
        {
            int currentNumber = numbers[i];
            int previousNumber = numbers[i - 1];
            bool isIncreasing = currentNumber > previousNumber;

            if (!isIncreasing)
                currentStartIndex = i;

            int currentSubsequenceLength = i - currentStartIndex + 1;
            if (currentSubsequenceLength > bestSubsequenceLength)
            {
                bestStartIndex = currentStartIndex;
                bestSubsequenceLength = currentSubsequenceLength;
            }
        }

        return string.Join(' ', numbers.Skip(bestStartIndex).Take(bestSubsequenceLength));
    }
}
