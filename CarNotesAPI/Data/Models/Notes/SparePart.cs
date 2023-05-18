using CarNotesAPI.Services;
using Neo4j.Driver;

namespace CarNotesAPI.Data.Models.Notes
{
    public class SparePart : Note
    {
        public override string NoteType => nameof(SparePart);

        public override string NoteTitle => $"{NoteType} ({Category}): {Name}";

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
            CurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

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

        /// <summary>
        /// Initializes a new instance of <see cref="SparePart"/>.
        /// </summary>
        public SparePart()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SparePart"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public SparePart(Dictionary<string, object> node)
        {
            Id                  = new Guid((string)node["id"]);
            Category            = (string)node["category"];
            OrderDate           = node.TryGetValue("order_date", out object? orderDate)
                                    ? ((LocalDate)orderDate).ToDateOnly()
                                    : null;
            PurchaseDate        = node.TryGetValue("order_date", out object? purchaseDate)
                                    ? ((LocalDate)purchaseDate).ToDateOnly()
                                    : null;
            Group               = node.TryGetValue("group", out object? group)
                                    ? (string)group
                                    : null;
            Name                = (string)node["name"];
            UoM                 = node.TryGetValue("uom", out object? uom)
                                    ? (string)uom
                                    : null;
            IsOE                = node.TryGetValue("is_oe", out object? isOE)
                                    ? (bool)isOE
                                    : null;
            OENumber            = node.TryGetValue("oe_number", out object? oeNumber)
                                    ? (string)oeNumber
                                    : null;
            ReplacementNumber   = node.TryGetValue("replacement_number", out object? replacementNumber)
                                    ? (string)replacementNumber
                                    : null;
            Manufacturer        = node.TryGetValue("manufacturer", out object? manufacturer)
                                    ? (string)manufacturer
                                    : null;
            CountryOfOrigin     = node.TryGetValue("country_of_origin", out object? countryOfOrigin)
                                    ? (string)countryOfOrigin
                                    : null;
            Qty                 = Convert.ToInt32(node["qty"]);
            Price               = (double)node["price"];
            ShopWebsiteUrl      = node.TryGetValue("shop_website_url", out object? shopWebsiteUrl)
                                    ? (string)shopWebsiteUrl
                                    : null;
            ShopAddress         = node.TryGetValue("shop_address", out object? shopAddress)
                                    ? (string)shopAddress
                                    : null;
            ProductionDate      = node.TryGetValue("production_date", out object? productionDate)
                                    ? ((LocalDate)productionDate).ToDateOnly()
                                    : null;
            ExpirationDate      = node.TryGetValue("expiration_date", out object? expirationDate)
                                    ? ((LocalDate)expirationDate).ToDateOnly()
                                    : null;
            Comment             = node.TryGetValue("comment", out object? comment)
                                    ? (string)comment
                                    : null;
        }
    }
}
