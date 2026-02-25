namespace EnglishCentralManagement.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        private static readonly TimeSpan VnOffset = TimeSpan.FromHours(7);

        /// <summary>
        /// Dùng khi LƯU DB – convert về UTC
        /// </summary>
        public static DateTimeOffset ToUtcDb(this DateTimeOffset dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Dùng khi GET RA – convert về giờ VN (+07)
        /// </summary>
        public static DateTimeOffset ToVnTime(this DateTimeOffset dateTime)
        {
            return dateTime.ToOffset(VnOffset);
        }
    }
}
