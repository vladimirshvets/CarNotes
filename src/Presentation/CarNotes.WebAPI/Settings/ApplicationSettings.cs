namespace CarNotes.WebAPI.Settings
{
    public class ApplicationSettings
    {
        public const string SectionName = "ApplicationSettings";

        public string JwtSecret { get; set; } = string.Empty;

        public string WebServerUrl { get; set; } = string.Empty;

        public string WebClientUrl { get; set; } = string.Empty;
    }
}
