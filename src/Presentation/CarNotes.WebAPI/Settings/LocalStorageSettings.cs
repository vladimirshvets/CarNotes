namespace CarNotes.WebAPI.Settings
{
    public class LocalStorageSettings
    {
        public const string SectionName = "LocalStorageSettings";

        public string StoragePath { get; set; } = string.Empty;
    }
}
