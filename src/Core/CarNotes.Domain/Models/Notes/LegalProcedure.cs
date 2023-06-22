using CarNotes.Domain.Interfaces.Services;

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
    }
}
