namespace CarNotesAPI.ViewModels
{
    public class LegalProcedureViewModel : MileageViewModel
    {
        public string? Group { get; set; }

        public string Title { get; set; } = string.Empty;

        public double TotalAmount { get; set; }

        public DateOnly? ExpirationDate { get; set; }
    }
}
