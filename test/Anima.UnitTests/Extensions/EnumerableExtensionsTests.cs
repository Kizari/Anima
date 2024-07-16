using Anima.Extensions;

namespace Anima.UnitTests.Extensions;

public class EnumerableExtensionsTests
{
    [Theory]
    [MemberData(nameof(TestEnumerables))]
    public void ToListedSentence_ReturnsExpectedResult(IEnumerable<string> values, string conjunction, string expected)
    {
        var result = values.ToListedSentence(conjunction);
        Assert.Equal(expected, result);
    }
    
    public static IEnumerable<object[]> TestEnumerables =
    [
        [new[] {"One", "Two", "Three"}, "and", "One, Two and Three"],
        [new[] {"Four", "Five", "Six", "Seven"}, "or", "Four, Five, Six or Seven"]
    ];
}