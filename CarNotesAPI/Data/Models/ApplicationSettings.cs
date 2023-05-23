namespace CarNotesAPI.Data.Models
{
    public class ApplicationSettings
    {
        public Uri? Neo4jConnection { get; set; }

        public string Neo4jUser { get; set; } = string.Empty;

        public string Neo4jPassword { get; set; } = string.Empty;

        public string Neo4jDatabase { get; set; } = string.Empty;

        public string JwtSecret { get; set; } = string.Empty;

        public string WebServerUrl { get; set; } = string.Empty;

        public string WebClientUrl { get; set; } = string.Empty;
    }
}
