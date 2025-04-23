namespace Anima.Extensions.Console.Command;

/// <summary>
/// Represents a command that can be executed from a console application.
/// </summary>
public interface IConsoleCommandBase
{
    /// <summary>
    /// Path to the command in the console command menu tree.
    /// </summary>
    /// <remarks>
    /// Each menu should be separated by a <c>/</c> character. If no <c>/</c> characters are present, the command
    /// will be added to the root menu. If <c>/</c> characters are present, the substring after the final <c>/</c>
    /// character will be the text of the menu item that represents this command.
    /// </remarks>
    string Path { get; }
    
    /// <summary>
    /// User-facing description of what the command does when executed. This will be displayed in the console.
    /// </summary>
    string Description { get; }
}