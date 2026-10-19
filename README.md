# Longest Increasing Subsequence

[![CI](https://github.com/AlexNadalin/2467fe1c-42af-4756-a353-ce1d28b45744/actions/workflows/ci.yml/badge.svg)](https://github.com/AlexNadalin/2467fe1c-42af-4756-a353-ce1d28b45744/actions/workflows/ci.yml)

A C# .NET solution that takes a string of integers separated by single spaces and returns the longest increasing subsequence found in it. If more than one subsequence shares the longest length, the earliest one is returned.

```
Input:  6 1 5 9 2
Output: 1 5 9
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

Check your installed version with:

```bash
dotnet --version
```

## Verifying the solution

All commands below are run from the repository root.

### Run the unit tests

```bash
dotnet test
```

This builds both projects and runs the provided test cases in `CodeTest.Tests/TestData`, plus additional tests for edge cases such as ties, equal numbers, empty input and invalid input. A successful run ends with:

```
Passed!  - Failed:     0, Passed:    24, Skipped:     0, Total:    24
```

To see the result of each individual test case:

```bash
dotnet test --logger "console;verbosity=normal"
```

### Run the console app

```bash
dotnet run --project CodeTest
```

Enter a sequence of numbers when prompted and the result is printed:

```
Enter numbers separated by spaces: 6 1 5 9 2
1 5 9
```

### Using Visual Studio

1. Open `CodeTest.slnx`.
2. Open **Test > Test Explorer** and select **Run All Tests**.
3. To try the console app, set `CodeTest` as the startup project and press **F5**.

## Linting

Code style and quality rules are defined in `.editorconfig` and enforced by the .NET SDK's built-in analyzers, configured for every project in `Directory.Build.props`. Rule violations appear as warnings when building and are highlighted in Visual Studio as you type.

Check for lint warnings:

```bash
dotnet build
```

Check formatting without changing any files:

```bash
dotnet format --verify-no-changes
```

Automatically fix formatting and code style issues:

```bash
dotnet format
```

In CI (where the `CI` environment variable is `true`), any warning fails the build.

## Code coverage

Coverage is collected with [Coverlet](https://github.com/coverlet-coverage/coverlet) and turned into a report with [ReportGenerator](https://github.com/danielpalme/ReportGenerator), which is installed as a local dotnet tool. The settings in `coverage.runsettings` measure the solution code only. `Program.cs` is excluded because it only handles console input and output.

Restore the report tool (first time only):

```bash
dotnet tool restore
```

Run the tests with coverage and generate the report:

```bash
dotnet test --settings coverage.runsettings --results-directory TestResults
dotnet reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:"Html;TextSummary"
```

Open `coveragereport/index.html` in a browser to see line-by-line coverage, or read the summary in `coveragereport/Summary.txt`. The current coverage is **100%** of lines and branches.

Delete the `TestResults` folder before re-running if you only want the latest results in the report.

## Continuous integration

A [GitHub Actions workflow](.github/workflows/ci.yml) runs on every push and pull request to `main`, and can also be started manually from the **Actions** tab. It:

1. Checks formatting with `dotnet format --verify-no-changes`.
2. Builds the solution, failing on any lint warning.
3. Runs all unit tests with code coverage.
4. Adds a coverage summary to the run's summary page.
5. Uploads the test results and full HTML coverage report as a downloadable artifact named `test-results-and-coverage`.

The badge at the top of this README shows the result of the latest run.

## How it works

The function makes a single pass over the numbers, tracking where the current increasing run started:

1. Each number is compared with the one before it.
2. If it is larger, the current run continues. Otherwise, a new run starts at that number.
3. Whenever the current run becomes strictly longer than the best run found so far, it becomes the new best. Because a run of equal length never replaces the best, the earliest run wins ties.

For `6 1 5 9 2` the runs are `6`, `1 5 9` and `2`, so the result is `1 5 9`.

The solution runs in **O(n)** time, as every number is visited exactly once.

### Assumptions

- **"Subsequence" means adjacent numbers.** The numbers in the result must appear next to each other in the input. This is confirmed by the provided test cases, e.g. `6 2 4 6 1 5 9 2` returns `2 4 6` rather than `2 4 6 9`.
- **Increasing means strictly increasing.** Equal adjacent numbers end a run, so `1 1 1` returns `1`.
- **Empty or whitespace-only input** returns an empty string.
- **Invalid input** throws a `FormatException` for a value that is not an integer, or an `OverflowException` for a value outside the range of an `int`. The console app catches these and prints a friendly message.

## Project structure

```
├── .github/workflows/ci.yml          GitHub Actions CI workflow
├── .editorconfig                     Code style and lint rules
├── Directory.Build.props             Analyzer settings shared by all projects
├── coverage.runsettings              Code coverage settings
├── dotnet-tools.json                 Local dotnet tools (ReportGenerator)
├── CodeTest/                         Console app and solution code
│   ├── Program.cs                    Reads input from the console and prints the result
│   └── SubsequenceFinder.cs          LongestIncreasingSubsequence implementation
└── CodeTest.Tests/                   xUnit test project
    ├── SubsequenceFinderTests.cs     Provided test cases and edge case tests
    ├── SubsequenceFinderTestData.cs  Discovers and loads test cases from TestData
    └── TestData/
        └── ReturnsLongestIncreasingSubsequence/   Data for the test of the same name
            └── TestCase01/
                ├── Input.txt
                └── ExpectedOutput.txt
```

## Adding a test case

Test cases are discovered automatically, so no code changes are needed:

1. Create a new folder in `CodeTest.Tests/TestData/ReturnsLongestIncreasingSubsequence` named `TestCase` followed by the next two-digit number, e.g. `TestCase12`.
2. Add an `Input.txt` file containing the space-separated numbers.
3. Add an `ExpectedOutput.txt` file containing the expected result.
4. Run `dotnet test`.
