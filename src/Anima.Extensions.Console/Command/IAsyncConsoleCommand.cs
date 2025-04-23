namespace Anima.Extensions.Console.Command;

/// <summary>
/// Represents an asynchronous command that can be executed from within a console application.
/// </summary>
public interface IAsyncConsoleCommand : IConsoleCommandBase
{
    /// <summary>
    /// The actions to perform when executing this command.
    /// </summary>
    Task ExecuteAsync();
}