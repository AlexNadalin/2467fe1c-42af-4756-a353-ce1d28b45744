using System.Globalization;

namespace CodeTest.Tests;

// Each test case is a folder in TestData/ReturnsLongestIncreasingSubsequence containing
// Input.txt and ExpectedOutput.txt. New cases are picked up automatically by adding a folder
// named TestCase<case_number>, with the number zero-padded to two digits.
public static class SubsequenceFinderTestData
{
    private static readonly string TestDataPath = Path.Combine(
        AppContext.BaseDirectory,
        "TestData",
        nameof(SubsequenceFinderTests.ReturnsLongestIncreasingSubsequence));

    public static TheoryData<string> TestCases
    {
        get
        {
            var testCases = new TheoryData<string>();

            var testCaseNames = Directory.GetDirectories(TestDataPath, "TestCase*")
                .Select(Path.GetFileName)
                .OfType<string>()
                .OrderBy(name => int.Parse(name["TestCase".Length..], CultureInfo.InvariantCulture));

            foreach (var testCaseName in testCaseNames)
                testCases.Add(testCaseName);

            return testCases;
        }
    }

    public static string ReadInput(string testCaseName) =>
        ReadTestFile(testCaseName, "Input.txt");

    public static string ReadExpectedOutput(string testCaseName) =>
        ReadTestFile(testCaseName, "ExpectedOutput.txt");

    private static string ReadTestFile(string testCaseName, string fileName) =>
        File.ReadAllText(Path.Combine(TestDataPath, testCaseName, fileName)).Trim();
}
