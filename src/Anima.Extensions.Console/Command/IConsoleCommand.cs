namespace Anima.Extensions.Console.Command;

/// <summary>
/// Represents a synchronous command that can be executed from within a console application.
/// </summary>
public interface IConsoleCommand : IConsoleCommandBase
{
    /// <summary>
    /// The actions to perform when executing this command.
    /// </summary>
    void Execute();
}