namespace CarNotes.Domain.Models
{
    public class FileObject
    {
        public MemoryStream Stream { get; set; } = new MemoryStream();

        public string FileName { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string FullName
            => System.IO.Path.Combine(Path, FileName);
    }
}
