namespace Anima.Extensions.Console.Application;

/// <summary>
/// Represents a factory that creates instances of <see cref="IConsoleApplicationBuilder"/>.
/// </summary>
public interface IConsoleApplicationBuilderFactory
{
    /// <summary>
    /// Instantiates a new uninitialized <see cref="IConsoleApplicationBuilder"/>.
    /// </summary>
    IConsoleApplicationBuilder Create();
}