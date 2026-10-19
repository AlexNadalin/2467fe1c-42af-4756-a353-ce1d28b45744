using CodeTest;

Console.Write("Enter numbers separated by spaces: ");
string input = Console.ReadLine() ?? "";
try
{
    Console.WriteLine(SubsequenceFinder.LongestIncreasingSubsequence(input));
}
catch (FormatException)
{
    Console.WriteLine("Invalid input: please enter whole numbers separated by spaces, e.g. 6 1 5 9 2");
}
catch (OverflowException)
{
    Console.WriteLine($"Invalid input: numbers must be between {int.MinValue} and {int.MaxValue}.");
}
