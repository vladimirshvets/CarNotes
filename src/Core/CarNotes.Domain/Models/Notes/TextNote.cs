namespace CarNotes.Domain.Models.Notes
{
    public class TextNote : Note
    {
        public override string NoteType => nameof(TextNote);

        public override string NoteTitle => $"{Title} ({Tag})";

        /// <summary>
        /// Text note title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Note tag.
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Note text.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
