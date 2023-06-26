using CarNotes.Domain.Models;

namespace CarNotes.FileStorage.AwsS3
{
    public class S3FileObject : FileObject
    {
        public string? BucketName { get; set; }
    }
}
