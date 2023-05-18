using CarNotesAPI.Services;

namespace CarNotesAPI.Data.Models.Notes
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
        /// Is washing contact?
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
            CurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Initializes a new instance of <see cref="Washing"/>.
        /// </summary>
        public Washing()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Washing"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public Washing(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Title = node.TryGetValue("title", out object? title) ? (string)title : null;
            Address = node.TryGetValue("address", out object? address) ? (string)address : null;
            IsContact = node.TryGetValue("is_contact", out object? isContact) ? (bool)isContact : null;
            IsDegreaserUsed = node.TryGetValue("is_degreaser_used", out object? isDegreaserUser) ? (bool)isDegreaserUser : null;
            IsPolishUsed = node.TryGetValue("is_polish_used", out object? isPolishUsed) ? (bool)isPolishUsed : null;
            IsAntiRainUsed = node.TryGetValue("is_antirain_used", out object? isAntiRainUsed) ? (bool)isAntiRainUsed : null;
            IsInteriorCleaned = node.TryGetValue("is_interior_cleaned", out object? isInteriorCleaned) ? (bool)isInteriorCleaned : null;
            TotalAmount = (double)node["total_amount"];
            Comment = node.TryGetValue("comment", out object? comment) ? (string)comment : null;
        }
    }
}
