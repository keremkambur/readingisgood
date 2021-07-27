using System;

namespace ReadingIsGood.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTimeOffset _epoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        ///     in seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixEpochInSeconds(this DateTime date)
        {
            return (long) (date.ToUniversalTime() - _epoch).TotalSeconds;
        }

        /// <summary>
        ///     in seconds; return 0 if datetime is null
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixEpochInSeconds(this DateTime? date)
        {
            return date?.ToUnixEpochInSeconds() ?? 0L;
        }
    }
}