namespace CarNotesAPI.ViewModels
{
    public class LegalProcedureViewModel : NoteViewModel
    {
        public string? Group { get; set; }

        public string Title { get; set; } = string.Empty;

        public double TotalAmount { get; set; }

        public DateOnly? ExpirationDate { get; set; }
    }
}
