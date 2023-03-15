namespace CarNotesAPI.ViewModels
{
    public class MileageViewModel
    {
        public Guid CarId { get; set; }

        public int OdometerValue { get; set; }

        public DateOnly Date { get; set; }
    }
}
