using System.Globalization;
using PortableCliApp;

namespace PortableCliApp.Tests;

/// <summary>
/// Contains unit tests for the reusable business functions defined in
/// <see cref="ApplicationFunctions"/>.
///
/// These tests use xUnit:
///
/// - [Fact] marks a test that runs once with one fixed set of test data.
/// - [Theory] marks a parameterized test that runs once for every [InlineData]
///   row supplied to it.
///
/// Most tests follow the Arrange-Act-Assert pattern:
///
/// 1. Arrange: prepare the input values and expected result.
/// 2. Act: call the method being tested.
/// 3. Assert: verify that the actual result matches the expected behavior.
///
/// Keeping each test focused on one behavior makes failures easier to diagnose
/// when the tests are run locally or from a CI pipeline such as Jenkins.
/// </summary>
public sealed class ApplicationFunctionsTests
{
    /// <summary>
    /// Verifies that CreateGreeting removes leading and trailing whitespace
    /// from a valid name and places the normalized name in the greeting.
    /// </summary>
    [Fact]
    public void CreateGreeting_WithValidName_TrimsNameAndReturnsGreeting()
    {
        // Arrange:
        // Supply a valid name containing extra whitespace at both ends.
        // The expected behavior is that CreateGreeting trims this whitespace.
        const string name = "  Messi  ";

        // Act:
        // Call the production method and capture the returned greeting.
        string result = ApplicationFunctions.CreateGreeting(name);

        // Assert:
        // Confirm that the returned value contains the trimmed name and
        // exactly matches the expected greeting text.
        Assert.Equal("Hello, Messi!", result);
    }

    /// <summary>
    /// Verifies that CreateGreeting rejects all forms of missing input:
    /// a null reference, an empty string and a whitespace-only string.
    /// </summary>
    /// <param name="name">
    /// One invalid name value supplied by the corresponding InlineData row.
    /// </param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateGreeting_WithMissingName_ThrowsArgumentException(
        string? name)
    {
        // Arrange:
        // No separate setup is required because xUnit supplies the invalid
        // value through the name parameter for each InlineData row.

        // Act:
        // Wrap the method call in an Action instead of executing it directly.
        // This allows Assert.Throws to invoke the operation and inspect the
        // exception that it produces.
        Action operation = () => ApplicationFunctions.CreateGreeting(name);

        // Assert:
        // Confirm that every invalid input causes an ArgumentException.
        // The test passes only when this exact exception type is thrown.
        Assert.Throws<ArgumentException>(operation);
    }

    /// <summary>
    /// Verifies integer addition with several representative data sets,
    /// including positive values, negative values and zero.
    /// </summary>
    /// <param name="firstNumber">The first integer operand.</param>
    /// <param name="secondNumber">The second integer operand.</param>
    /// <param name="expected">The expected sum for this test case.</param>
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(20, 22, 42)]
    [InlineData(-10, 4, -6)]
    [InlineData(0, 0, 0)]
    public void Add_WithTwoIntegers_ReturnsExpectedTotal(
        int firstNumber,
        int secondNumber,
        int expected)
    {
        // Arrange:
        // The test inputs and expected result are supplied directly by each
        // InlineData row, so no additional setup is needed.

        // Act:
        // Add the two supplied integers by calling the production method.
        int result = ApplicationFunctions.Add(firstNumber, secondNumber);

        // Assert:
        // Compare the actual sum with the expected value for this data row.
        // xUnit reports which parameter set failed if the values differ.
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that IsEven correctly classifies zero, positive integers and
    /// negative integers as either even or odd.
    /// </summary>
    /// <param name="number">The integer to classify.</param>
    /// <param name="expected">
    /// The expected Boolean result: true for even and false for odd.
    /// </param>
    [Theory]
    [InlineData(0, true)]
    [InlineData(2, true)]
    [InlineData(-4, true)]
    [InlineData(7, false)]
    [InlineData(-9, false)]
    public void IsEven_WithInteger_ReturnsExpectedResult(
        int number,
        bool expected)
    {
        // Arrange:
        // xUnit supplies both the number and its expected classification from
        // each InlineData row.

        // Act:
        // Ask the production method whether the supplied integer is even.
        bool result = ApplicationFunctions.IsEven(number);

        // Assert:
        // Verify that the returned Boolean value matches the expected result.
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that CalculateAverage returns the arithmetic mean for a
    /// non-empty collection of integers.
    /// </summary>
    [Fact]
    public void CalculateAverage_WithSeveralNumbers_ReturnsExpectedAverage()
    {
        // Arrange:
        // Prepare four integers whose sum is 64.
        // Dividing 64 by 4 gives the expected average of 16.0.
        int[] values = [10, 14, 19, 21];

        // Act:
        // Calculate the average by passing the array to the production method.
        double result = ApplicationFunctions.CalculateAverage(values);

        // Assert:
        // Confirm that the returned double equals the expected arithmetic mean.
        Assert.Equal(16.0, result);
    }

    /// <summary>
    /// Verifies that CalculateAverage rejects an empty collection because an
    /// arithmetic average cannot be calculated without at least one value.
    /// </summary>
    [Fact]
    public void CalculateAverage_WithEmptyCollection_ThrowsArgumentException()
    {
        // Arrange:
        // Create an empty integer array to represent a collection with no data.
        int[] values = [];

        // Act:
        // Store the method call in an Action so that Assert.Throws can execute
        // it and verify the exception produced.
        Action operation =
            () => ApplicationFunctions.CalculateAverage(values);

        // Assert:
        // Confirm that an empty collection causes an ArgumentException.
        Assert.Throws<ArgumentException>(operation);
    }

    /// <summary>
    /// Verifies that CalculateAverage distinguishes a null collection reference
    /// from an empty collection and throws ArgumentNullException for null.
    /// </summary>
    [Fact]
    public void CalculateAverage_WithNullCollection_ThrowsArgumentNullException()
    {
        // Arrange:
        // No variable is required because null is passed directly to the method.

        // Act:
        // Wrap the call in an Action so that the exception can be asserted
        // without allowing it to escape and terminate the test unexpectedly.
        Action operation =
            () => ApplicationFunctions.CalculateAverage(null);

        // Assert:
        // Confirm that a null collection reference produces the more specific
        // ArgumentNullException rather than a general ArgumentException.
        Assert.Throws<ArgumentNullException>(operation);
    }

    /// <summary>
    /// Verifies that BuildApplicationPath creates a path whose final components
    /// use the directory separator appropriate for the current operating system.
    ///
    /// This makes the test portable across Windows and Linux build agents.
    /// </summary>
    [Fact]
    public void BuildApplicationPath_UsesCurrentPlatformDirectorySeparator()
    {
        // Arrange:
        // The two relative path components are passed directly in the Act step.
        // The expected suffix is constructed separately below using Path.Combine.

        // Act:
        // Build the full application path. The returned path begins with
        // AppContext.BaseDirectory and ends with the supplied relative parts.
        string result = ApplicationFunctions.BuildApplicationPath(
            "data",
            "application-message.txt");

        /*
         * Build the expected ending with Path.Combine instead of hard-coding
         * a Windows path such as:
         *
         *     data\application-message.txt
         *
         * or a Linux path such as:
         *
         *     data/application-message.txt
         *
         * Path.Combine automatically uses the correct separator for whichever
         * operating system is executing the test.
         */
        string expectedSuffix = Path.Combine(
            "data",
            "application-message.txt");

        // Assert:
        // Check only the path suffix because the beginning of the returned path
        // contains AppContext.BaseDirectory, which varies between machines,
        // operating systems, build configurations and CI workspace locations.
        //
        // StringComparison.Ordinal performs a direct character-by-character
        // comparison without culture-specific rules.
        Assert.EndsWith(expectedSuffix, result, StringComparison.Ordinal);
    }
}
