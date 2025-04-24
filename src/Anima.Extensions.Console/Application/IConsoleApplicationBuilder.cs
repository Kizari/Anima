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
    /// Customizes the exit command for the application's menu.
    /// </summary>
    /// <param name="path">Path to the exit command within the menu tree.</param>
    /// <param name="description">Text description to display beneath the exit command.</param>
    /// <param name="onSelected">
    /// Optional action to execute when the command is selected.
    /// The default behaviour of terminating the menu loop will still apply.
    /// </param>
    /// <returns>A reference to this builder.</returns>
    public IConsoleApplicationBuilder WithCustomExitCommand(string path, string description, Action? onSelected = null);

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