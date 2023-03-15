using Neo4j.Driver;

namespace CarNotesAPI.Data.Models
{
    public class Mileage
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Odometer value.
        /// </summary>
        public int OdometerValue { get; set; }

        /// <summary>
        /// Populates a mileage instance from the set of fields.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        /// <returns>A new instance of mileage.</returns>
        public static Mileage FromNode(Dictionary<string, object> node)
        {
            Mileage mileage = new()
            {
                Id              = new Guid((string)node["id"]),
                Date            = ((LocalDate)node["date"]).ToDateOnly(),
                OdometerValue   = (int)(long)node["odometer"]
            };

            return mileage;
        }
    }
}
