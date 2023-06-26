namespace CarNotes.Domain.Interfaces.Services
{
    public interface IImageService
    {
        Task<string?> SaveCarAvatarAsync(
            Stream fileStream, string fileExtension, Guid carId);
    }
}
