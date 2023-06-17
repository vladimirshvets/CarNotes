using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    public interface IAuthService
    {
        string GenerateTokenString(User user);

        Task<bool> CheckPasswordAsync(User user, string password);

        string HashPassword(User user, string password);
    }
}
