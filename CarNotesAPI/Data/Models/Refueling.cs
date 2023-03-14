namespace CarNotesAPI.Data.Models
{
    public class Refueling
    {
        public DateOnly Date { get; set; }

        public double Volume { get; set; }

        public string? Comment { get; set; }
    }
}
