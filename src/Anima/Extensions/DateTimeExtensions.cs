namespace Anima.Extensions;

/// <summary>
/// Extension methods related to the <see cref="DateTime"/> type.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Rounds a <see cref="DateTime"/> to the closest time block of the given size.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to round.</param>
    /// <param name="block">The size of the time block to round to.</param>
    /// <returns>The rounded <see cref="DateTime"/>.</returns>
    public static DateTime RoundToNearest(this DateTime dateTime, TimeSpan block)
    {
        var delta = dateTime.Ticks % block.Ticks;
        var roundUp = delta > block.Ticks / 2;
        var offset = roundUp ? block.Ticks : 0;
        return new DateTime(dateTime.Ticks + offset - delta, dateTime.Kind);
    }
}