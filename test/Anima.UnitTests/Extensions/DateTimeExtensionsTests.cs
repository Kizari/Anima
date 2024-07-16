using Anima.Extensions;

namespace Anima.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [MemberData(nameof(TestDates))]
    public void RoundToNearest_ReturnsExpectedResult(DateTime dateTime, TimeSpan block, DateTime expected)
    {
        var result = dateTime.RoundToNearest(block);
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> TestDates() =>
    [
        [
            new DateTime(2020, 1, 1, 11, 0, 0), 
            TimeSpan.FromDays(1),
            new DateTime(2020, 1, 1)
        ],
        [
            new DateTime(2020, 1, 1, 13, 0, 0), 
            TimeSpan.FromDays(1),
            new DateTime(2020, 1, 2)
        ],
        [
            new DateTime(2020, 1, 1, 11, 0, 0),
            TimeSpan.FromHours(12),
            new DateTime(2020, 1, 1, 12, 0, 0)
        ]
    ];
}