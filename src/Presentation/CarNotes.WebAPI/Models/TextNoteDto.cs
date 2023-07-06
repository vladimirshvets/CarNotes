namespace CarNotes.WebAPI.Models
{
    public class TextNoteDto : NoteDto
    {
        public string Title { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;
    }
}
