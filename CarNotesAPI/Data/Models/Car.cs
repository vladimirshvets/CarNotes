namespace CarNotesAPI.Data.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string? Generation { get; set; }

        public string? VIN { get; set; }

        public int? Year { get; set; }

        public string? Plate { get; set; }
    }
}
