using CarNotes.Domain.Enums;

namespace CarNotes.Domain.Models
{
    public class Car
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Make.
        /// </summary>
        public string Make { get; set; } = string.Empty;

        /// <summary>
        /// Model.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Generation.
        /// </summary>
        public string? Generation { get; set; }

        /// <summary>
        /// Vehicle Identification Number.
        /// </summary>
        public string? VIN { get; set; }

        /// <summary>
        /// Year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Plate number.
        /// </summary>
        public string? Plate { get; set; }

        /// <summary>
        /// Engine type.
        /// </summary>
        public EngineType? EngineType { get; set; }

        /// <summary>
        /// Engine type represented as a string.
        /// </summary>
        public string? EngineTypeText => EngineType?.ToString();

        /// <summary>
        /// Vehicle in possession since, date.
        /// </summary>
        public DateOnly? OwnedFrom { get; set; }

        /// <summary>
        /// Vehicle in possession till, date.
        /// </summary>
        public DateOnly? OwnedTo { get; set; }

        /// <summary>
        /// Avatar URL.
        /// </summary>
        public string? AvatarUrl { get; set; }
    }
}
