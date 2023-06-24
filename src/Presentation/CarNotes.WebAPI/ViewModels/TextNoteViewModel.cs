namespace CarNotes.WebAPI.ViewModels
{
    public class TextNoteViewModel : NoteViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;
    }
}
