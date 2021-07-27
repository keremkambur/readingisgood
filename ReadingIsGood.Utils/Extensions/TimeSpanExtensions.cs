using System;

namespace ReadingIsGood.Utils.Extensions
{
    public static class TimeSpanExtensions
    {
        public static DateTime ToDateTimeFromUtcNow(this TimeSpan timeSpan)
        {
            return DateTime.UtcNow.AddTicks(timeSpan.Ticks);
        }
    }
}