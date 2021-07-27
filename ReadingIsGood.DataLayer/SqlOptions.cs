namespace ReadingIsGood.DataLayer
{
    public class SqlOptions
    {
        /// <summary>
        ///     for local development only!
        /// </summary>
        public string ConnectionString { get; set; }

        public bool UseLoggerFactory { get; set; }
    }
}