using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    /// <summary>
    /// Returns a user record by email.
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>An instance of user.</returns>
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
