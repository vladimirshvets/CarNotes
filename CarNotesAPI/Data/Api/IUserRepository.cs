using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api
{
    public interface IUserRepository
    {
        /// <summary>
        /// Returns an user record by username.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>An instance of user if exists.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Returns an user record by email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>An instance of user if exists.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Returns an user record by user ID.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>An instance of user if exists.</returns>
        Task<User?> GetAsync(Guid userId);

        /// <summary>
        /// Creates a new user record.
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>A newly created instance of user.</returns>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Updates an existing user record.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="user">Updated user data</param>
        /// <returns>Updated instance of user.</returns>
        Task<User> UpdateAsync(Guid userId, User user);

        /// <summary>
        /// Returns total number of users.
        /// </summary>
        /// <returns>Total number of users.</returns>
        Task<int> GetNumberOfUsersAsync();
    }
}
