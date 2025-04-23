using Anima.Extensions.Console.Command;

namespace Anima.Extensions.Console.Menu;

/// <summary>
/// Represents a menu item in a console application's menu/submenu that executes a command upon selection.
/// </summary>
internal interface ICommandNode : IMenuNode
{
    /// <summary>
    /// Command to execute when this menu item is selected.
    /// </summary>
    IConsoleCommandBase Command { get; }
}