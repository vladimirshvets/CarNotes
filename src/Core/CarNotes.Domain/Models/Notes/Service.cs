using System.Text;
using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class Service : Note
    {
        public override string NoteType => nameof(Service);

        public override string NoteTitle
        {
            get
            {
                var builder = new StringBuilder(Title);
                string location = string.Join(", ", StationName, Address);
                if (location.Length > 0)
                {
                    builder.Append($" ({location})");
                }
                builder.Append($" | BYN {TotalAmount:F2}");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Service title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Service station name.
        /// </summary>
        public string? StationName { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Website URL.
        /// </summary>
        public string? WebsiteUrl { get; set; }

        /// <summary>
        /// Cost of work.
        /// </summary>
        public double CostOfWork { get; set; }

        /// <summary>
        /// Cost of spare parts.
        /// </summary>
        public double CostOfSpareParts { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount => CostOfWork + CostOfSpareParts;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");
    }
}
