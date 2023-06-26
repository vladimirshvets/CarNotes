using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;

namespace CarNotes.FileStorage.Local;

public class LocalStorage : IFileStorageService<FileObject>
{
    private readonly StorageOptions _storageOptions;

    public LocalStorage(StorageOptions storageOptions)
    {
        _storageOptions = storageOptions;
    }
    public async Task<FileStorageResponse> UploadFileAsync(FileObject fileObject)
    {
        var response = new FileStorageResponse();

        string path = Path.Combine(_storageOptions.StoragePath, fileObject.Path);
        try {
            Directory.CreateDirectory(path);
            using var fileStream = new FileStream(
                Path.Combine(path, fileObject.FileName), FileMode.Create);

            fileObject.Stream.Seek(0, SeekOrigin.Begin);
            await fileObject.Stream.CopyToAsync(fileStream);

            response.StatusCode = 201;
            response.Message = $"{fileObject.FullName} has been uploaded sucessfully.";
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }
}
