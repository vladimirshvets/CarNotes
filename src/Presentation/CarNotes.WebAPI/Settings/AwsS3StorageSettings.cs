namespace CarNotes.WebAPI.Settings
{
    public class AwsS3StorageSettings
    {
        public const string SectionName = "AwsS3StorageSettings";

        public string AccessKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string BucketName { get; set; } = string.Empty;
    }
}
