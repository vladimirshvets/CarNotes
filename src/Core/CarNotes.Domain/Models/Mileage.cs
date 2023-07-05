namespace CarNotes.Domain.Models
{
    public class Mileage
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Odometer value.
        /// </summary>
        public int OdometerValue { get; set; }

        /// <summary>
        /// Related notes.
        /// </summary>
        public virtual IEnumerable<Note>? Notes { get; set; }
    }
}
