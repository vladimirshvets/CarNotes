using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class Washing : Note
    {
        public override string NoteType => nameof(Service);

        public override string NoteTitle => $"{NoteType}: {Title}, {Address} ({TotalAmount} BYN)";

        /// <summary>
        /// Washing title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Is contact washing?
        /// </summary>
        public bool? IsContact { get; set; }

        /// <summary>
        /// Is degreaser used?
        /// </summary>
        public bool? IsDegreaserUsed { get; set; }

        /// <summary>
        /// Is polish used?
        /// </summary>
        public bool? IsPolishUsed { get; set; }

        /// <summary>
        /// Is anti-rain used?
        /// </summary>
        public bool? IsAntiRainUsed { get; set; }

        /// <summary>
        /// Is interior cleaned?
        /// </summary>
        public bool? IsInteriorCleaned { get; set; }

        /// <summary>
        /// Price total.
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");
    }
}
