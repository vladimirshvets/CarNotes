namespace CarNotesAPI.Data.Models
{
    public class Mileage
    {
        public DateOnly Date { get; set; }

        public int Value { get; set; }

        public string? Comment { get; set; }
    }
}
