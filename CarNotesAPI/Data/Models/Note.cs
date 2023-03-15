namespace CarNotesAPI.Data.Models
{
    public abstract class Note
    {
        /// <summary>
        /// Note identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Mileage marker.
        /// </summary>
        public virtual Mileage? Mileage { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string? Comment { get; set; }
    }
}
