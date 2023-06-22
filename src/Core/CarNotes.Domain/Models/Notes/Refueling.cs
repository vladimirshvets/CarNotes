using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class Refueling : Note
    {
        public override string NoteType => nameof(Refueling);

        public override string NoteTitle => $"Refueling: {Volume} l. * {Price}";

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
