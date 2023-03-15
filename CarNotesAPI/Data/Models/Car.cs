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
        public string Make { get; set; }

        /// <summary>
        /// Model.
        /// </summary>
        public string Model { get; set; }

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

        /// /// <summary>
        /// Populates a car from the set of fields.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        /// <returns>A new instance of car.</returns>
        public static Car FromNode(Dictionary<string, object> node)
        {
            Car car = new()
            {
                Id = new Guid((string)node["id"]),
                Make = node.ContainsKey("make") ? (string)node["make"] : string.Empty,
                Model = node.ContainsKey("model") ? (string)node["model"] : string.Empty,
                Generation = node.ContainsKey("generation") ? (string)node["generation"] : null,
                VIN = node.ContainsKey("VIN") ? (string)node["VIN"] : null,
                Year = node.ContainsKey("year") ? (int)(long)node["year"] : null,
                Plate = node.ContainsKey("plate") ? (string)node["plate"] : null
            };

            return car;
        }
    }
}
