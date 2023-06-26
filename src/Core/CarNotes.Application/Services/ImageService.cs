using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;

namespace CarNotes.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IFileStorageService<FileObject> _fileStorage;

        private readonly ICarRepository _carRepository;

        public ImageService(
            IFileStorageService<FileObject> fileStorage,
            ICarRepository carRepository)
        {
            _fileStorage = fileStorage;
            _carRepository = carRepository;
        }

        public async Task<string?> SaveCarAvatarAsync(
            Stream fileStream, string fileExtension, Guid carId)
        {
            var fileObject = new FileObject
            {
                FileName = $"avatar{fileExtension}",
                Path = $"cars/{carId}"
            };
            await fileStream.CopyToAsync(fileObject.Stream);
            fileObject.Stream.Seek(0, SeekOrigin.Begin);

            FileStorageResponse response =
                await _fileStorage.UploadFileAsync(fileObject);

            if (response.StatusCode != 201)
            {
                return null;
            }

            await _carRepository.SetAvatarUrlAsync(carId, fileObject.FullName);

            return fileObject.FullName;
        }
    }
}
