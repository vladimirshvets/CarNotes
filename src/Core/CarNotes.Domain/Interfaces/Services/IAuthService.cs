using CarNotes.Domain.Models;

namespace CarNotes.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(User user);

        Task<bool> CheckPasswordAsync(User user, string password);

        string HashPassword(User user, string password);
    }
}
