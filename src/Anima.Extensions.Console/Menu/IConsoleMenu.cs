namespace Anima.Extensions.Console.Menu;

/// <summary>
/// Represents a tree of menus for a console application.
/// </summary>
public interface IConsoleMenu
{
    /// <summary>
    /// Steps the user through the menu recursively, until a command is selected or the exit option is selected.
    /// </summary>
    /// <returns><c>true</c> if the menu was exited through the exit option, otherwise <c>false</c>.</returns>
    Task<bool> RunAsync();
}