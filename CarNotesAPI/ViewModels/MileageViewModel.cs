using CarNotesAPI.Data.Models;

namespace CarNotesAPI.ViewModels
{
    public class MileageViewModel
    {
        public Guid CarId { get; set; }

        // ToDo:
        // delete Date and OdometerValue, use Mileage instead.

        public DateOnly Date { get; set; }

        public int OdometerValue { get; set; }

        public Mileage Mileage { get; set; }
    }
}
