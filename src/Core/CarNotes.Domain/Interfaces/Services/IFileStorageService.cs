using CarNotes.Domain.Models;

namespace CarNotes.Domain.Interfaces.Services
{
    public interface IFileStorageService<T> where T : FileObject
    {
        Task<FileStorageResponse> UploadFileAsync(T fileObject);
    }
}
