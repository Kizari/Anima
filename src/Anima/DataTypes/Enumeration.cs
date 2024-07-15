using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Anima.DataTypes;

/// <summary>
/// An enumeration-like type that allows for member types not supported by C#'s <c>enum</c> keyword.
/// </summary>
/// <typeparam name="TValue">The type of the enumeration members.</typeparam>
/// <example>
/// The following example demonstrates how to create a string-backed enumeration for three
/// numbers and overload the cast operator to allow for explicit conversion from <c>string</c>.<br/>
/// <code>
/// public class Numbers : Enumeration&lt;string&gt;
/// {
///    private Numbers(string value) : base(value) { }
///    
///    public static Numbers One { get; } = new("One");
///    public static Numbers Two { get; } = new("Two");
///    public static Numbers Three { get; } = new("Three");
///    
///    public static implicit operator Numbers(string value) => FromValue&lt;Numbers&gt;(value);
/// }
/// </code>
/// </example>
public abstract class Enumeration<TValue> : IComparable
    where TValue : IComparable
{
    protected Enumeration(TValue value)
    {
        Value = value;
    }

    /// <summary>
    /// The value of this enumeration member.
    /// </summary>
    private TValue Value { get; }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj == this;

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc />
    public override string? ToString() => Value.ToString();

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        // ReSharper disable once PossibleUnintendedReferenceComparison
        if (obj == this)
        {
            return 0;
        }

        if (obj == null)
        {
            return 1;
        }

        if (GetType() != obj.GetType())
        {
            throw new ArgumentException("Cannot compare objects of different type", nameof(obj));
        }

        return Value.CompareTo(obj);
    }

    public static implicit operator TValue(Enumeration<TValue> value) => value.Value;

    /// <summary>
    /// Finds the enumeration member with the matching value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <typeparam name="TEnum">The type of the enumeration to search.</typeparam>
    /// <returns>The enumeration member matching the given value.</returns>
    /// <exception cref="ArgumentException">Throws if no enumeration member has the given value.</exception>
    protected static TEnum FromValue<TEnum>(TValue value) where TEnum : Enumeration<TValue>
    {
        var result = typeof(TEnum)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Select(property => (Enumeration<TValue>)property.GetValue(null)!)
            .FirstOrDefault(other => other.Value.Equals(value));

        return result == null
            ? throw new ArgumentException($"No matching enumeration member found for {value}", nameof(value))
            : (TEnum)result;
    }
}