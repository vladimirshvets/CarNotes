using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Domain.Models.Notes
{
    public class Service : Note
    {
        public override string NoteType => nameof(Service);

        public override string NoteTitle => $"{NoteType}: {Title}";

        /// <summary>
        /// Washing title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Service station name.
        /// </summary>
        public string? StationName { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Website URL.
        /// </summary>
        public string? WebsiteUrl { get; set; }

        /// <summary>
        /// Cost of work.
        /// </summary>
        public double CostOfWork { get; set; }

        /// <summary>
        /// Cost of spare parts.
        /// </summary>
        public double CostOfSpareParts { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount => CostOfWork + CostOfSpareParts;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            ICurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Initializes a new instance of <see cref="Service"/>.
        /// </summary>
        public Service()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Service"/>
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public Service(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Title = (string)node["title"];
            StationName = node.TryGetValue("station_name", out object? stationName) ? (string)stationName : null;
            Address = node.TryGetValue("address", out object? address) ? (string)address : null;
            WebsiteUrl = node.TryGetValue("website_url", out object? websiteUrl) ? (string)websiteUrl : null;
            CostOfWork = (double)node["cost_of_work"];
            CostOfSpareParts = (double)node["cost_of_spare_parts"];
            Comment = node.TryGetValue("comment", out object? comment) ? (string)comment : null;
        }
    }
}
