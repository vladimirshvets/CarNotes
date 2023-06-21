using CarNotes.Domain.Interfaces.Services;
using Neo4j.Driver;

namespace CarNotes.Domain.Models.Notes
{
    public class LegalProcedure : Note
    {
        public override string NoteType => nameof(LegalProcedure);

        public override string NoteTitle => $"{NoteType}: {Title}";

        /// <summary>
        /// Washing title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Group (kind) of procedure.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Expiration date.
        /// </summary>
        public DateOnly? ExpirationDate { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="LegalProcedure"/>.
        /// </summary>
        public LegalProcedure()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LegalProcedure"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public LegalProcedure(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Group = node.TryGetValue("group", out object? group) ? (string)group : null;
            Title = (string)node["title"];
            TotalAmount = (double)node["total_amount"];
            ExpirationDate = node.TryGetValue("expiration_date", out object? expirationDate) ? ((LocalDate)expirationDate).ToDateOnly() : null;
            Comment = node.TryGetValue("comment", out object? comment) ? (string)comment : null;
        }
    }
}
