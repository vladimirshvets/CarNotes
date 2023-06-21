namespace CarNotes.Persistence.Neo4j
{
    public class Neo4jOptions
    {
        public Uri? Connection { get; set; }

        public string? User { get; set; }

        public string? Password { get; set; }

        public string? Database { get; set; }
    }
}
