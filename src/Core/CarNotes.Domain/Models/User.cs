using Neo4j.Driver;

namespace CarNotes.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="User"/>.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="User"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public User(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            UserName = node.TryGetValue("username", out object? userName) ? (string)userName : string.Empty;
            Email = node.TryGetValue("email", out object? email) ? (string)email : string.Empty;
            PasswordHash = node.TryGetValue("password_hash", out object? passwordHash) ? (string)passwordHash : string.Empty;
            FirstName = node.TryGetValue("firstname", out object? firstName) ? (string)firstName : string.Empty;
            LastName = node.TryGetValue("lastname", out object? lastName) ? (string)lastName : string.Empty;
            CreatedAt = ((ZonedDateTime)node["created_at"]).ToDateTimeOffset().DateTime;
            UpdatedAt = ((ZonedDateTime)node["updated_at"]).ToDateTimeOffset().DateTime;
        }
    }
}
