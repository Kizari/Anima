using Anima.Extensions.Console.Application;
using Anima.Extensions.Console.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace Anima.Extensions.Console;

/// <summary>
/// Extensions methods for consumers.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds <c>Anima.Extensions.Console</c> services to the given service collection.
    /// </summary>
    /// <param name="services">Service collection to add the services to.</param>
    /// <returns>Same collection as the input parameter.</returns>
    public static IServiceCollection AddAnimaConsole(this IServiceCollection services) => services
        .AddSingleton<IConsoleApplicationBuilderFactory, ConsoleApplicationBuilderFactory>()
        .AddSingleton<IConsoleMenu, ConsoleMenu>();
}