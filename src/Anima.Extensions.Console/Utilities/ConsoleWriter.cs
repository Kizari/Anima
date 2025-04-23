using System.Runtime.CompilerServices;

namespace Anima.Extensions.Console.Utilities;

/// <summary>
/// Simple helper class for writing colored messages to the console.
/// </summary>
public static class ConsoleWriter
{
    /// <summary>
    /// Writes a line of text to the console.
    /// </summary>
    /// <param name="text">Message to display in the console.</param>
    /// <param name="color">Color of the message, will use the current color if null.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(string text, ConsoleColor? color = null)
    {
        if (color == null)
        {
            System.Console.WriteLine(text);
        }
        else
        {
            System.Console.ForegroundColor = color.Value;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
        }
    }

    /// <summary>
    /// Writes multiple lines of text to the console.
    /// </summary>
    /// <param name="color">Color to display the messages in, will use the current color if null.</param>
    /// <param name="lines">The lines of text to display in the console.</param>
    public static void WriteLines(ConsoleColor? color = null, params string[] lines)
    {
        if (color == null)
        {
            foreach (var line in lines)
            {
                System.Console.WriteLine(line);
            }
        }
        else
        {
            System.Console.ForegroundColor = color.Value;
        
            foreach (var line in lines)
            {
                System.Console.WriteLine(line);
            }
        
            System.Console.ResetColor();
        }
    }

    /// <summary>
    /// Appends text to the console.
    /// </summary>
    /// <param name="text">Message to display in the console.</param>
    /// <param name="color">Color of the message, will use the current color if null.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string text, ConsoleColor? color = null)
    {
        if (color == null)
        {
            System.Console.Write(text);
        }
        else
        {
            System.Console.ForegroundColor = color.Value;
            System.Console.Write(text);
            System.Console.ResetColor();
        }
    }
}