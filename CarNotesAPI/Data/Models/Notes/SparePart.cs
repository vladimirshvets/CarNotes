namespace CarNotesAPI.Data.Models.Notes
{
    public class SparePart : Note
    {
        public override string NoteType => nameof(SparePart);

        public override string NoteTitle => $"Spare Part: ";

        public DateOnly? OrderDate { get; set; }

        public DateOnly? PurchaseDate { get; set; }

        public Mileage? InstallationMileage
        {
            get
            {
                return Mileage;
            }

            set
            {
                Mileage = value;
            }
        }

        public Mileage? RemovalMileage { get; set; }

        public string? Group { get; set; }

        public string Name { get; set; } = string.Empty;

        // ...
    }
}
