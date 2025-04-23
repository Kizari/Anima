namespace Anima.Extensions.Console.Application;

/// <summary>
/// Represents a builder for an <see cref="IConsoleApplication"/>.
/// </summary>
public interface IConsoleApplicationBuilder
{
    /// <summary>
    /// Displays a title when a run method is called on the resulting <see cref="IConsoleApplication"/>.
    /// </summary>
    /// <param name="title">Title text to display to the user.</param>
    /// <param name="color">Color to display the title in.</param>
    /// <returns>A reference to this builder.</returns>
    /// <example>
    /// Using the title <c>"My Application"</c> would result in the following output:
    /// <code>
    /// ============================
    ///  M Y  A P P L I C A T I O N
    /// ============================
    /// </code>
    /// </example>
    IConsoleApplicationBuilder WithTitle(string title, ConsoleColor color = ConsoleColor.Gray);

    /// <summary>
    /// Builds the application.
    /// </summary>
    /// <returns>The configured <see cref="IConsoleApplication"/> instance.</returns>
    /// <remarks>
    /// Call <see cref="IConsoleApplication.RunAsync"/> or <see cref="IConsoleApplication.RunOnceAsync"/>
    /// on the returned <see cref="IConsoleApplication"/> to execute the resulting application.
    /// </remarks>
    IConsoleApplication Build();
}