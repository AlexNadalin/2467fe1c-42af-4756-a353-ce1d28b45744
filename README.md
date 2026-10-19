# Longest Increasing Subsequence

[![CI](https://github.com/AlexNadalin/2467fe1c-42af-4756-a353-ce1d28b45744/actions/workflows/ci.yml/badge.svg)](https://github.com/AlexNadalin/2467fe1c-42af-4756-a353-ce1d28b45744/actions/workflows/ci.yml)

A C# .NET solution that takes a string of integers separated by whitespace and returns the longest increasing subsequence found in it. If more than one subsequence shares the longest length, the earliest one is returned.

```
Input:  6 1 5 9 2
Output: 1 5 9
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started/) (optional, only needed to run the solution in a container)

Check your installed .NET version with:

```bash
dotnet --version
```

## Verifying the solution

All commands below are run from the repository root.

### Run the unit tests

```bash
dotnet test
```

This builds both projects and runs the provided test cases in `CodeTest.Tests/TestData`, plus additional tests for edge cases such as ties, equal numbers, tabs and other whitespace, empty input and invalid input, and tests for the console app's output and exit codes. A successful run ends with:

```
Passed!  - Failed:     0, Passed:    36, Skipped:     0, Total:    36
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

Input can also be piped in, which is useful for scripts:

```bash
echo "6 1 5 9 2" | dotnet run --project CodeTest
```

Only the result is written to standard output. The prompt and any error messages are written to standard error, so they don't mix with the result. The app exits with code `0` on success and `1` if the input is invalid.

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

Coverage is collected with [Coverlet](https://github.com/coverlet-coverage/coverlet) and turned into a report with [ReportGenerator](https://github.com/danielpalme/ReportGenerator), which is installed as a local dotnet tool. The settings in `coverage.runsettings` measure the solution code only, including the console app logic in `ConsoleApp.cs`. `Program.cs` is excluded because it is a single line that passes the real console streams to `ConsoleApp.Run`.

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

## Docker

The `Dockerfile` builds the solution, runs the unit tests and packages the console app into a small runtime-only image. If any test fails, the image is not built.

Build the image:

```bash
docker build --tag longest-increasing-subsequence .
```

Run the console app interactively and enter numbers when prompted:

```bash
docker run --rm -it longest-increasing-subsequence
```

Or pipe the input in directly:

```bash
echo "6 1 5 9 2" | docker run --rm -i longest-increasing-subsequence
```

To run only the unit tests inside a container, build the `test` stage:

```bash
docker build --target test .
```

## Continuous integration

A [GitHub Actions workflow](.github/workflows/ci.yml) runs on every push and pull request to `main`, and can also be started manually from the **Actions** tab. It:

1. Checks formatting with `dotnet format --verify-no-changes`.
2. Builds the solution, failing on any lint warning.
3. Runs all unit tests with code coverage.
4. Adds a coverage summary to the run's summary page.
5. Uploads the test results and full HTML coverage report as a downloadable artifact named `test-results-and-coverage`.
6. Builds the Docker image (which runs the tests again inside the container) and checks the container returns `1 5 9` for the input `6 1 5 9 2`, and exits with a non-zero code for invalid input.

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
- **Any whitespace separates numbers.** Spaces, tabs and new lines are all accepted, and repeated whitespace is treated as a single separator. The result is always separated by single spaces.
- **Empty or whitespace-only input** returns an empty string.
- **Invalid input** throws a `FormatException` for a value that is not an integer, or an `OverflowException` for a value outside the range of an `int`. The console app catches these, writes a friendly message to standard error and exits with code `1`.

## Project structure

```
├── .github/workflows/ci.yml          GitHub Actions CI workflow
├── Dockerfile                        Builds, tests and packages the console app
├── .dockerignore                     Files excluded from the Docker build
├── .editorconfig                     Code style and lint rules
├── Directory.Build.props             Analyzer settings shared by all projects
├── coverage.runsettings              Code coverage settings
├── dotnet-tools.json                 Local dotnet tools (ReportGenerator)
├── CodeTest/                         Console app and solution code
│   ├── Program.cs                    Entry point, connects ConsoleApp to the real console
│   ├── ConsoleApp.cs                 Reads input, writes the result or an error, returns the exit code
│   └── SubsequenceFinder.cs          LongestIncreasingSubsequence implementation
└── CodeTest.Tests/                   xUnit test project
    ├── ConsoleAppTests.cs            Console app output, error and exit code tests
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
