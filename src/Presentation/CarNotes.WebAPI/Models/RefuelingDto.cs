namespace CarNotes.WebAPI.Models
{
    public class RefuelingDto : NoteDto
    {
        public double Volume { get; set; }

        public double Price { get; set; }

        public string? Distributor { get; set; }

        public string? Address { get; set; }
    }
}
