namespace CarNotesAPI.ViewModels
{
    public class RefuelingViewModel : NoteViewModel
    {
        public double Volume { get; set; }

        public double Price { get; set; }

        public string? Distributor { get; set; }

        public string? Address { get; set; }
    }
}
