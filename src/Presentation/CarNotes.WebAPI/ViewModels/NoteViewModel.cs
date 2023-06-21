using CarNotes.Domain.Models;

namespace CarNotes.WebAPI.ViewModels
{
    public class NoteViewModel
    {
        public Guid CarId { get; set; }

        public Mileage Mileage { get; set; }

        public string? Comment { get; set; }
    }
}
