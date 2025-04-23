using Anima.Extensions.Console.Menu;

namespace Anima.Extensions.Console.Application;

/// <inheritdoc />
public class ConsoleApplicationBuilderFactory(IConsoleMenu menu) : IConsoleApplicationBuilderFactory
{
    /// <inheritdoc />
    public IConsoleApplicationBuilder Create()
    {
        return new ConsoleApplicationBuilder(menu);
    }
}