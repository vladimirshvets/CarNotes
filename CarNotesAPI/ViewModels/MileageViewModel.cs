namespace CarNotesAPI.ViewModels
{
    public class MileageViewModel
    {
        public Guid CarId { get; set; }

        public int Value { get; set; }

        public DateOnly Date { get; set; }

        public string? Comment { get; set; }
    }
}
