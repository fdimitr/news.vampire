namespace News.Vampire.Service.Logger
{
    public class DbLoggerOptions
    {
        public string? ConnectionString { get; set; }
        public required string[] LogFields { get; init; }
        public required string LogTable { get; init; }
    }
}
