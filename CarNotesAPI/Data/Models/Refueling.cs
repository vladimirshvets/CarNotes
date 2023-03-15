using CarNotesAPI.Data.Api;
using CarNotesAPI.Services;

namespace CarNotesAPI.Data.Models
{
    public class Refueling : Note
    {
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
        public double Total => Price * Volume;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotal =>
            CurrencyService.Convert(Total, Mileage?.Date, "USD");

        /// <summary>
        /// Trading network name.
        /// </summary>
        public string? Distributor { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public override string Title => $"Refueling: {Volume} l. * {Price}";

        /// <summary>
        /// Populates a refueling from the set of fields.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        /// <returns>A new instance of refueling.</returns>
        public static Refueling FromNode(Dictionary<string, object> node)
        {
            Refueling refueling = new()
            {
                Id = new Guid((string)node["id"]),
                Volume = (double)node["volume"],
                Price = (double)node["price"],
                Distributor = node.ContainsKey("distributor") ? (string)node["distributor"] : null,
                Address = node.ContainsKey("address") ? (string)node["address"] : null,
                Comment = node.ContainsKey("comment") ? (string)node["comment"] : null
            };

            return refueling;
        }
    }
}
