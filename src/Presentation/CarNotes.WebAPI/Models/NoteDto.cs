using CarNotes.Domain.Models;

namespace CarNotes.WebAPI.Models
{
    public class NoteDto
    {
        public Guid CarId { get; set; }

        public Mileage Mileage { get; set; }

        public string? Comment { get; set; }
    }
}
