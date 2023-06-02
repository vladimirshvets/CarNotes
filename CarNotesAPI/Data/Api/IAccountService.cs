using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    public interface IAccountService
    {
        Task<User?> FindByEmailAsync(string email);

        Task<User?> GetAsync(Guid userId);

        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(Guid userId, User user);

        string ProduceJWToken(User user);

        string HashPassword(User user, string password);

        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
