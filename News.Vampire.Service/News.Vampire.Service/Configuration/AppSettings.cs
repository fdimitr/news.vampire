namespace News.Vampire.Service.Configuration
{
    public class AppSettings
    {
        public const string Section = "AppSettings";
        // Application Core
        public int MaxConcurrencyDownloadSessions { get; set; }

        // Amazon
        public string AmazonAccessKey { get; set; } = string.Empty;
        public string AmazonSecretKey { get; set; } = string.Empty;

        public string AmazonPublicS3BucketName { get; set; } = string.Empty;
        public string AmazonS3AwsUrl { get; set; } = string.Empty;
        public string AmazonS3RegionEndpoint { get; set; } = string.Empty;
    }
}
