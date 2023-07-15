namespace CarNotes.Persistence.Neo4j
{
    public class Neo4jOptions
    {
        public const string SectionName = "Neo4jSettings";

        public Uri Connection { get; set; }

        public string User { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Database { get; set; } = string.Empty;
    }
}
