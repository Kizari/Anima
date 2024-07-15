using System.Text;

namespace Anima.Extensions;

/// <summary>
/// Extension methods related to the <see cref="IEnumerable{T}"/> type.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Generates a list sentence <c>string</c> from an enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable to generate the sentence from.</param>
    /// <param name="conjunction">The conjunction to use before the last item in the enumerable.</param>
    /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
    /// <returns>The list sentence.</returns>
    /// <example>
    /// Given the following enumerable:<br/>
    /// <code>
    /// List&lt;string&gt; strings = ["One", "Two", "Three"];
    /// </code>
    /// <br/>Using the following code:<br/>
    /// <code>
    /// Console.WriteLine(strings.ToListedSentence("or"));
    /// </code>
    /// <br/>Will output the following:<br/>
    /// <code>
    /// One, Two or Three
    /// </code>
    /// </example>
    public static string ToListedSentence<T>(this IEnumerable<T> enumerable, string conjunction = "and")
    {
        var builder = new StringBuilder();
        var array = enumerable as T[] ?? enumerable.ToArray(); // Avoids multiple enumeration

        for (var i = 0; i < array.Length; i++)
        {
            if (i == array.Length - 1 && i > 0)
            {
                builder.Append($" {conjunction} ");
            }
            else if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(array[i]);
        }

        return builder.ToString();
    }
}