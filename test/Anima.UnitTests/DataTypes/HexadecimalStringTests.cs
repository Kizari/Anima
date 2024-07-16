using Anima.DataTypes;

namespace Anima.UnitTests.DataTypes;

public class HexadecimalStringTests
{
    [Theory]
    [InlineData("A0 BD 5C Z3 BB")]
    [InlineData("A0BD5C_D3BB")]
    public void Constructor_WithInvalidString_ThrowsArgumentException(string hexString)
    {
        Assert.Throws<ArgumentException>(() => new HexadecimalString(hexString));
    }

    [Theory]
    [MemberData(nameof(ValidStrings))]
    public void Constructor_WithValidString_ProducesCorrectValues(string hexString)
    {
        var result = new HexadecimalString(hexString);
        Assert.Equal("A0BD5CD3BB21", result.ToString());
    }

    [Theory]
    [InlineData(new byte[] {0x12, 0x55, 0xB3, 0xFF, 0x4C})]
    public void Constructor_WithByteArray_ProducesCorrectValue(byte[] bytes)
    {
        var result = new HexadecimalString(bytes);
        Assert.Equal("1255B3FF4C", result.ToString());
    }

    [Theory]
    [MemberData(nameof(ValidStrings))]
    public void OperatorHexadecimalString_FromString_ProducesCorrectValue(string hexString)
    {
        var result = (HexadecimalString)hexString;
        Assert.Equal("A0BD5CD3BB21", result.ToString());
    }

    [Theory]
    [InlineData(new byte[] {0x12, 0x55, 0xB3, 0xFF, 0x4C})]
    public void OperatorHexadecimalString_FromBytes_ProducesCorrectValue(byte[] bytes)
    {
        var result = (HexadecimalString)bytes;
        Assert.Equal("1255B3FF4C", result.ToString());
    }

    [Theory]
    [InlineData("40ac 673d 11fc")]
    public void OperatorString_ProducesCorrectValue(string hexString)
    {
        var result = new HexadecimalString(hexString);
        Assert.Equal("40AC673D11FC", (string)result);
    }
    
    [Theory]
    [InlineData("40ac 673d 11fc")]
    public void OperatorByteArray_ProducesCorrectValue(string hexString)
    {
        var result = new HexadecimalString(hexString);
        Assert.Equal([0x40, 0xAC, 0x67, 0x3D, 0x11, 0xFC], (byte[])result);
    }

    public static IEnumerable<object[]> ValidStrings =>
    [
        ["a0bd5cD3bb21"],
        ["A0BD5CD3BB21"],
        ["A0 BD 5C D3 BB 21"],
        ["A0BD 5CD3 BB21"],
        ["a0-bd-5c-d3-bb-21"],
        [" A0 BD-5C D3BB 21 "]
    ];
}