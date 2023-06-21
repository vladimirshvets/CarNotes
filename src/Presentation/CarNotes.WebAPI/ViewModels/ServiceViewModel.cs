namespace CarNotes.WebAPI.ViewModels
{
    public class ServiceViewModel : NoteViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string? StationName { get; set; }

        public string? Address { get; set; }

        public string? WebsiteUrl { get; set; }

        public double CostOfWork { get; set; }

        public double CostOfSpareParts { get; set; }
    }
}
