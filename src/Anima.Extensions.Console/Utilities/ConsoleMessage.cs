namespace Anima.Extensions.Console.Utilities;

/// <summary>
/// Simple text message that is to be displayed in a console, with an optional display color.
/// </summary>
internal class ConsoleMessage(string text, ConsoleColor? color = null)
{
    /// <summary>
    /// Text to display in the console.
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// Color to display the text in.
    /// </summary>
    public ConsoleColor? Color { get; } = color;
}