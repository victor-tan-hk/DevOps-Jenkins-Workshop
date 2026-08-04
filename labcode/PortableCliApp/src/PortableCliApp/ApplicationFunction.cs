using System.Globalization;

namespace PortableCliApp;

/// <summary>
/// Contains the application's reusable business functions.
///
/// Keeping these functions separate from Program.cs makes them easy to
/// call from both the console application and the unit-test project.
/// </summary>
public static class ApplicationFunctions
{
    /// <summary>
    /// Creates a friendly greeting for a supplied name.
    /// </summary>
    /// <param name="name">
    /// The person's name. Leading and trailing whitespace is removed.
    /// </param>
    /// <returns>
    /// A greeting containing the normalized name.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the name is null, empty or consists only of whitespace.
    /// </exception>
    public static string CreateGreeting(string? name)
    {
        /*
         * Validate user-supplied input rather than allowing an unclear
         * greeting such as "Hello, !".
         */
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "A non-empty name must be provided.",
                nameof(name));
        }

        /*
         * Trim removes leading and trailing whitespace in a
         * platform-independent manner.
         */
        string normalizedName = name.Trim();

        return $"Hello, {normalizedName}!";
    }

    /// <summary>
    /// Adds two integer values.
    /// </summary>
    public static int Add(int firstNumber, int secondNumber)
    {
        return firstNumber + secondNumber;

        /// Comment out the previous statement
        /// and uncomment the one below to introduce an error
        
        /// return firstNumber - secondNumber;

    }

    /// <summary>
    /// Determines whether an integer is even.
    /// </summary>
    public static bool IsEven(int number)
    {
        /*
         * An integer is even when division by two leaves no remainder.
         * This also works for negative numbers and zero.
         */
        return number % 2 == 0;

        /// Comment out the previous statement
        /// and uncomment the one below to introduce an error
        
        /// return number % 2 == 1;

    }

    /// <summary>
    /// Calculates the arithmetic average of a collection of integers.
    /// </summary>
    /// <param name="numbers">The numbers whose average will be calculated.</param>
    /// <returns>The arithmetic average as a double-precision number.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the collection reference is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the collection contains no values.
    /// </exception>
    public static double CalculateAverage(IEnumerable<int>? numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        /*
         * Materialize the sequence once. This avoids enumerating a
         * potentially computed sequence repeatedly.
         */
        int[] values = numbers.ToArray();

        if (values.Length == 0)
        {
            throw new ArgumentException(
                "At least one number must be provided.",
                nameof(numbers));
        }

        /*
         * Average returns a double for an integer sequence, preventing
         * loss of the fractional portion through integer division.
         */
        return values.Average();

        /// Comment out the previous statement
        /// and uncomment the one below to introduce an error
        
        /// return values.Sum();


    }



    /// <summary>
    /// Builds the full path to a resource distributed with the application.
    /// </summary>
    /// <param name="relativePathParts">
    /// Individual path components such as "data" and "message.txt".
    /// </param>
    public static string BuildApplicationPath(params string[] relativePathParts)
    {
        ArgumentNullException.ThrowIfNull(relativePathParts);

        if (relativePathParts.Length == 0)
        {
            throw new ArgumentException(
                "At least one relative path component is required.",
                nameof(relativePathParts));
        }

        /*
         * AppContext.BaseDirectory identifies the directory containing
         * the application's compiled output.
         *
         * Path.Combine uses the correct directory separator for the
         * current operating system:
         *
         * Windows: \
         * Linux:   /
         */
        string[] allPathParts =
        [
            AppContext.BaseDirectory,
            .. relativePathParts
        ];

        return Path.Combine(allPathParts);
    }
}