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
        /// Initializes a new instance of <see cref="Mileage"/>.
        /// </summary>
        public Mileage()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Mileage"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public Mileage(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Date = ((LocalDate)node["date"]).ToDateOnly();
            OdometerValue = (int)(long)node["odometer"];
        }
    }
}
