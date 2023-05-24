using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    public interface IUserRepository
    {
        /// <summary>
        /// Returns a user record by username.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>An instance of user if exists.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Returns a user record by email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>An instance of user if exists.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Creates a new user record.
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>A newly created instance of user.</returns>
        Task<User> AddAsync(User user);
    }
}
