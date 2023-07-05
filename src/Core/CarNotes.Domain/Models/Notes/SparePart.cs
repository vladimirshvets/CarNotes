using System.Text;
using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class SparePart : Note
    {
        public override string NoteType => nameof(SparePart);

        public override string NoteTitle
        {
            get
            {
                var builder = new StringBuilder($"{Name} ({Category})");
                if (Qty > 0)
                {
                    builder.Append($" * {Qty}");
                }
                builder.Append($" | BYN {TotalAmount:F2}");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Category.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Order date.
        /// </summary>
        public DateOnly? OrderDate { get; set; }

        /// <summary>
        /// Purchase date.
        /// </summary>
        public DateOnly? PurchaseDate { get; set; }

        /// <summary>
        /// Installation mileage.
        /// </summary>
        public Mileage? InstallationMileage
        {
            get
            {
                return Mileage;
            }
            set
            {
                Mileage = value;
            }
        }

        /// <summary>
        /// Removal mileage.
        /// </summary>
        public Mileage? RemovalMileage { get; set; }

        /// <summary>
        /// Group (kind) of spare part.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unit of measure.
        /// </summary>
        public string? UoM { get; set; }

        /// <summary>
        /// Is original equipment part?
        /// </summary>
        public bool? IsOE { get; set; }

        /// <summary>
        /// Original equipment number.
        /// </summary>
        public string? OENumber { get; set; }

        /// <summary>
        /// Replacement number.
        /// </summary>
        public string? ReplacementNumber { get; set; }

        /// <summary>
        /// Manufacturer.
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Country of origin.
        /// </summary>
        public string? CountryOfOrigin { get; set; }

        /// <summary>
        /// Price per unit, in national currency.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount => Price * Qty;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Shop website url.
        /// </summary>
        public string? ShopWebsiteUrl { get; set; }

        /// <summary>
        /// Shop address.
        /// </summary>
        public string? ShopAddress { get; set; }

        /// <summary>
        /// Production date.
        /// </summary>
        public DateOnly? ProductionDate { get; set; }

        /// <summary>
        /// Expiration date.
        /// </summary>
        public DateOnly? ExpirationDate { get; set; }
    }
}
