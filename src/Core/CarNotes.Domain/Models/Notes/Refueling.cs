using System.Text;
using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class Refueling : Note
    {
        public override string NoteType => nameof(Refueling);

        public override string NoteTitle
        {
            get
            {
                var builder = new StringBuilder($"{Volume} l. * {Price}");
                var secondaryInfo = new List<string>();
                if (Distributor != null)
                {
                    secondaryInfo.Add(Distributor);
                }
                if (Address != null)
                {
                    secondaryInfo.Add(Address);
                }
                string location = string.Join(", ", secondaryInfo);
                if (location.Length > 0)
                {
                    builder.Append($" ({location})");
                }
                builder.Append($" | BYN {TotalAmount:F2}");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Amount of fuel.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Price per unit, in national currency.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount => Price * Volume;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Trading network name.
        /// </summary>
        public string? Distributor { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }
    }
}
