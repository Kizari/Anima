using System.Globalization;
using System.Text.RegularExpressions;

namespace Anima.DataTypes;

/// <summary>
/// A string whose value represents hexadecimal data.
/// </summary>
public readonly partial struct HexadecimalString
{
    /// <summary>
    /// Regular expression that detects whitespace and hyphen characters as valid separators in a hexadecimal string.
    /// </summary>
    [GeneratedRegex(@"[\s-]")]
    private static partial Regex HexSeparatorRegex();
    
    /// <summary>
    /// Regular expression that determines that a string contains only valid hexadecimal characters.
    /// </summary>
    [GeneratedRegex(@"\A\b[0-9a-fA-F]+\b\Z")]
    private static partial Regex ValidHexRegex();
    
    /// <summary>
    /// The hexadecimal string.
    /// </summary>
    private readonly string _value;

    /// <summary>
    /// Instantiates a new <see cref="HexadecimalString"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The string containing the hexadecimal representation.</param>
    /// <exception cref="ArgumentException">Thrown if the string is not a valid hexadecimal representation.</exception>
    public HexadecimalString(string value)
    {
        var cleanedValue = HexSeparatorRegex().Replace(value, "").ToUpperInvariant();
        
        if (!ValidHexRegex().IsMatch(cleanedValue))
        {
            throw new ArgumentException($"Invalid hexadecimal string \"{value}\".", nameof(value));
        }

        if (cleanedValue.Length % 2 != 0)
        {
            throw new ArgumentException($"Hexadecimal string \"{value}\" does not have an even number " +
                                        $"of characters.", nameof(value));
        }

        _value = cleanedValue;
    }

    /// <summary>
    /// Instantiates a new <see cref="HexadecimalString"/> from an array of bytes.
    /// </summary>
    /// <param name="bytes">The bytes that this <see cref="HexadecimalString"/> will represent.</param>
    public HexadecimalString(byte[] bytes)
    {
        _value = BitConverter.ToString(bytes).Replace("-", "");
    }

    /// <inheritdoc />
    public override string ToString() => _value;

    public static implicit operator HexadecimalString(string value) => new(value);
    public static implicit operator HexadecimalString(byte[] bytes) => new(bytes);
    public static implicit operator string(HexadecimalString hexString) => hexString._value;
    public static implicit operator byte[](HexadecimalString hexString) => Enumerable.Range(0, hexString._value.Length)
        .Where(x => x % 2 == 0)
        .Select(x => byte.Parse(hexString._value.Substring(x, 2), NumberStyles.HexNumber))
        .ToArray();
}