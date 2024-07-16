using Anima.DataTypes;

namespace Anima.UnitTests.DataTypes;

public class EnumerationTests
{
    [Fact]
    public void Equals_WithMatchingMembers_ReturnsTrue()
    {
        var a = TestEnumeration.One;
        var b = TestEnumeration.One;
        Assert.True(a == b);
    }

    [Fact]
    public void ToString_ReturnsExpectedValue()
    {
        var two = TestEnumeration.Two;
        Assert.Equal("Two", two.ToString());
    }

    [Fact]
    public void OperatorTValue_ReturnsExpectedValue()
    {
        var three = TestEnumeration.Three;
        var result = (string)three;
        Assert.Equal("Three", result);
    }

    [Fact]
    public void CompareTo_WithInvalidOther_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TestEnumeration.One.CompareTo(12));
    }

    [Fact]
    public void CompareTo_WithValidOther_ReturnsExpectedResult()
    {
        var one = TestEnumeration.One;
        var two = TestEnumeration.Two;
        var three = TestEnumeration.Three;
        
        // Comparisons are based on string so it's alphabetical
        Assert.Equal(-1, one.CompareTo(two));
        Assert.Equal(1, two.CompareTo(three));
        Assert.Equal(0, one.CompareTo(TestEnumeration.One));
        Assert.Equal(1, three.CompareTo(null));
    }

    [Fact]
    public void FromValue_WithInvalidMember_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = (TestEnumeration)"Four";
        });
    }

    [Fact]
    public void FromValue_WithValidMember_GetsMemberWithSameReference()
    {
        var a = TestEnumeration.Two;
        var b = (TestEnumeration)"Two";
        Assert.True(a == b);
    }
    
    private class TestEnumeration(string value) : Enumeration<string>(value)
    {
        public static TestEnumeration One { get; } = new("One");
        public static TestEnumeration Two { get; } = new("Two");
        public static TestEnumeration Three { get; } = new("Three");
        
        public static explicit operator TestEnumeration(string value) => FromValue<TestEnumeration>(value);
    }
}