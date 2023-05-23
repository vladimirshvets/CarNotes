using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    public interface IAccountService
    {
        Task<User?> FindByEmailAsync(string email);

        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
