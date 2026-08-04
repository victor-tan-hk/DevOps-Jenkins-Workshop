using System.Runtime.InteropServices;
using PortableCliApp;

/*
 * Program.cs is the application's entry point.
 *
 * It invokes the reusable methods from ApplicationFunctions and writes
 * their results to the console.
 */

Console.WriteLine("Cross-platform .NET 10 CLI demonstration");
Console.WriteLine("========================================");
Console.WriteLine();

/*
 * Show the environment in which this build is running.
 *
 * RuntimeInformation provides cross-platform runtime information
 * without relying on operating-system-specific commands.
 */
Console.WriteLine("Runtime environment");
Console.WriteLine("-------------------");
Console.WriteLine($"Framework:            {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Operating system:     {RuntimeInformation.OSDescription}");
Console.WriteLine($"OS architecture:      {RuntimeInformation.OSArchitecture}");
Console.WriteLine($"Process architecture: {RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"Directory separator:  {Path.DirectorySeparatorChar}");
Console.WriteLine();

/*
 * Function 1: Produce a normalized greeting.
 */
string greeting = ApplicationFunctions.CreateGreeting(" Jenkins workshop participant ");

Console.WriteLine("Greeting function");
Console.WriteLine("-----------------");
Console.WriteLine(greeting);
Console.WriteLine();

/*
 * Function 2: Add two integers.
 */
const int firstNumber = 20;
const int secondNumber = 22;

int total = ApplicationFunctions.Add(firstNumber, secondNumber);

Console.WriteLine("Addition function");
Console.WriteLine("-----------------");
Console.WriteLine($"{firstNumber} + {secondNumber} = {total}");
Console.WriteLine();

/*
 * Function 3: Determine whether several numbers are even.
 */
int[] parityValues = [7, 10, -4, 0];

Console.WriteLine("Even-number function");
Console.WriteLine("--------------------");

foreach (int value in parityValues)
{
    Console.WriteLine($"{value} is even: {ApplicationFunctions.IsEven(value)}");
}

Console.WriteLine();

/*
 * Function 4: Calculate and display an average.
 */
int[] measurements = [10, 14, 19, 21];

double average = ApplicationFunctions.CalculateAverage(measurements);


Console.WriteLine("Average function");
Console.WriteLine("----------------");
Console.WriteLine($"Values:  {string.Join(", ", measurements)}");
Console.WriteLine($"Average: {average}");
Console.WriteLine();

/*
 * Load an application resource from the build output.
 *
 * The project file copies data/application-message.txt into the output
 * directory. AppContext.BaseDirectory and Path.Combine make the lookup
 * independent of:
 *
 * - the operating system;
 * - the user's home directory;
 * - the current working directory;
 * - Windows drive letters;
 * - directory separator differences.
 */
string messagePath = ApplicationFunctions.BuildApplicationPath(
    "data",
    "application-message.txt");

Console.WriteLine("Application resource");
Console.WriteLine("--------------------");
Console.WriteLine($"Resource path: {messagePath}");

if (File.Exists(messagePath))
{
    string resourceMessage = File.ReadAllText(messagePath);
    Console.WriteLine(resourceMessage);
}
else
{
    /*
     * Set a non-zero exit code so that a script or CI pipeline can detect
     * that the expected application resource was unavailable.
     */
    Console.Error.WriteLine(
        "The expected application resource could not be found.");

    Environment.ExitCode = 1;
}