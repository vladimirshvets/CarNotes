using Neo4j.Driver;

namespace CarNotesAPI.Data.Models
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
        public string? EngineTypeText => EngineType.ToString();

        /// <summary>
        /// Vehicle in possession since, date.
        /// </summary>
        public DateOnly? OwnedFrom { get; set; }

        /// <summary>
        /// Vehicle in possession till, date.
        /// </summary>
        public DateOnly? OwnedTo { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Car"/>.
        /// </summary>
        public Car()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Car"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public Car(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Make = node.TryGetValue("make", out object? make) ? (string)make : string.Empty;
            Model = node.TryGetValue("model", out object? model) ? (string)model : string.Empty;
            Generation = node.TryGetValue("generation", out object? generation) ? (string)generation : null;
            VIN = node.TryGetValue("VIN", out object? vin) ? (string)vin : null;
            Year = node.TryGetValue("year", out object? year) ? (int)(long)year : null;
            Plate = node.TryGetValue("plate", out object? plate) ? (string)plate : null;
            EngineType = node.TryGetValue("engine_type", out object? engineType) ? (EngineType)Enum.Parse(typeof(EngineType), (string)engineType, true) : null;
            OwnedFrom = node.TryGetValue("owned_from", out object? ownedFrom) ? ((LocalDate)ownedFrom).ToDateOnly() : null;
            OwnedTo = node.TryGetValue("owned_to", out object? ownedTo) ? ((LocalDate)ownedTo).ToDateOnly() : null;
        }
    }
}
