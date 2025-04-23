using Anima.Extensions.Console.Menu;
using Anima.Extensions.Console.Utilities;

namespace Anima.Extensions.Console.Application;

/// <inheritdoc />
internal class ConsoleApplication(IConsoleMenu menu) : IConsoleApplication
{
    internal ConsoleMessage? Title { get; set; }

    /// <inheritdoc />
    public async Task RunAsync()
    {
        DisplayTitle();
        while (!await menu.RunAsync());
        EndApplication();
    }

    /// <inheritdoc />
    public async Task RunOnceAsync()
    {
        DisplayTitle();
        await menu.RunAsync();
        EndApplication();
    }

    /// <summary>
    /// Displays the formatted representation of <see cref="Title"/> in the console.
    /// </summary>
    private void DisplayTitle()
    {
        if (Title != null)
        {
            var border = new string(Enumerable.Repeat('=', Title.Text.Length * 2 + 1).ToArray());
            var title = " " + string.Join(' ', Title.Text.ToUpper().ToCharArray());
            ConsoleWriter.WriteLines(Title.Color, border, title, border);
        }
    }

    /// <summary>
    /// Displays a final message and waits for the user to press a key before continuing.
    /// </summary>
    private static void EndApplication()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Press any key to exit...");
        System.Console.ReadKey();
    }
}