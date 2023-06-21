using CarNotes.Domain.Models;

namespace CarNotes.WebAPI.ViewModels
{
    public class SparePartViewModel : NoteViewModel
    {
        public string Category { get; set; } = string.Empty;

        public DateOnly? OrderDate { get; set; }

        public DateOnly? PurchaseDate { get; set; }

        public Mileage? RemovalMileage { get; set; }

        public string? Group { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? UoM { get; set; }

        public bool? IsOE { get; set; }

        public string? OENumber { get; set; }

        public string? ReplacementNumber { get; set; }

        public string? Manufacturer { get; set; }

        public string? CountryOfOrigin { get; set; }

        public double Price { get; set; }

        public int Qty { get; set; }

        public string? ShopWebsiteUrl { get; set; }

        public string? ShopAddress { get; set; }

        public DateOnly? ProductionDate { get; set; }

        public DateOnly? ExpirationDate { get; set; }
    }
}
