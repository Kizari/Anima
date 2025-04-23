using Anima.Extensions.Console.Command;

namespace Anima.Extensions.Console.Menu;

/// <inheritdoc />
internal class CommandNode(IConsoleCommandBase command) : ICommandNode
{
    /// <inheritdoc />
    public string Name { get; } = command.Path.Split('/')[^1];
    
    /// <inheritdoc />
    public IConsoleCommandBase Command { get; } = command;
}