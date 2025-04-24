using Anima.Extensions.Console.Menu;
using Anima.Extensions.Console.Utilities;

namespace Anima.Extensions.Console.Application;

/// <inheritdoc />
internal class ConsoleApplicationBuilder(IConsoleMenu menu) : IConsoleApplicationBuilder
{
    private bool _isCompleted;
    private readonly ConsoleApplication _application = new(menu);
    
    /// <inheritdoc />
    public IConsoleApplicationBuilder WithTitle(string title, ConsoleColor color = ConsoleColor.Gray)
    {
        _application.Title = new ConsoleMessage(title, color);
        return this;
    }

    /// <inheritdoc />
    public IConsoleApplicationBuilder WithCustomExitCommand(string path, string description, Action? onSelected = null)
    {
        ((ConsoleMenu)menu).CustomExitCommand = new ConsoleMenu.ExitCommand(path, description, onSelected);
        return this;
    }

    /// <inheritdoc />
    public IConsoleApplication Build()
    {
        if (_isCompleted)
        {
            throw new InvalidOperationException(
                "Cannot use a ConsoleApplicationBuilder that is already in a completed state.");
        }
        
        _isCompleted = true;
        return _application;
    }
}