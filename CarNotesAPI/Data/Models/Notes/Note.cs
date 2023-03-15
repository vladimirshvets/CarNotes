namespace CarNotesAPI.Data.Models.Notes
{
    public abstract class Note
    {
        /// <summary>
        /// Note identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Note type.
        /// </summary>
        public abstract string NoteType { get; }

        /// <summary>
        /// Note title.
        /// </summary>
        public abstract string NoteTitle { get; }

        /// <summary>
        /// Mileage marker.
        /// </summary>
        public virtual Mileage? Mileage { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string? Comment { get; set; }
    }
}
