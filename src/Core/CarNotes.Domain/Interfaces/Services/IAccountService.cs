using CarNotes.Domain.Models;

namespace CarNotes.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<User?> FindByEmailAsync(string email);

        Task<User?> GetAsync(Guid userId);

        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(Guid userId, User user);
    }
}
