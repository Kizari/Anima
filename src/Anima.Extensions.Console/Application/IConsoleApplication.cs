namespace Anima.Extensions.Console.Application;

/// <summary>
/// Represents a simple console application.
/// </summary>
public interface IConsoleApplication
{
    /// <summary>
    /// Continuously loops through the application menus until the user selects the exit command from the root menu.
    /// </summary>
    Task RunAsync();

    /// <summary>
    /// Loops through the menu tree once.
    /// Returns once the user has selected a command and that command has finished executing.
    /// </summary>
    Task RunOnceAsync();
}