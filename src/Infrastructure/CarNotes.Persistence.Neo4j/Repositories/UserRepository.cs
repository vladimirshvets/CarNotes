using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class UserRepository : IUserRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    public UserRepository(INeo4jDataAccess neo4jDataAccess)
    {
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        string query =
            @"MATCH (u:User { username: $username })
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "username", username }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "u" }, parameters);

        if (response.Count == 0)
        {
            return null;
        }

        return new User(response[0]);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string query =
            @"MATCH (u:User { email: $email })
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "email", email.ToLower() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "u" }, parameters);

        if (response.Count == 0)
        {
            return null;
        }

        return new User(response[0]);
    }

    public async Task<User?> GetAsync(Guid userId)
    {
        string query =
            @"MATCH (u:User { id: $userId })
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "u" }, parameters);

        if (response.Count == 0)
        {
            return null;
        }

        return new User(response[0]);
    }

    public async Task<User> AddAsync(User user)
    {
        string query =
            @"CREATE
                (u: User {
                    id: apoc.create.uuid(),
                    username: $userName,
                    email: $email,
                    password_hash: $passwordHash,
                    firstname: $firstName,
                    lastname: $lastName,
                    created_at: datetime(),
                    updated_at: datetime()
                })
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "userName", user.UserName },
            { "email", user.Email.ToLower() },
            { "passwordHash", user.PasswordHash },
            { "firstName", user.FirstName },
            { "lastName", user.LastName }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
            query, parameters);

        return new User(response);
    }

    public async Task<User> UpdateAsync(Guid userId, User user)
    {
        string query =
            @"MATCH (u:User { id: $userId })
            SET
                u.username = $userName,
                u.email = $email,
                u.password_hash = $passwordHash,
                u.firstname = $firstName,
                u.lastname = $lastName,
                u.updated_at = datetime()
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() },
            { "userName", user.UserName },
            { "email", user.Email },
            { "passwordHash", user.PasswordHash },
            { "firstName", user.FirstName },
            { "lastName", user.LastName }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
            query, parameters);

        return new User(response);
    }

    public async Task<int> GetNumberOfUsersAsync()
    {
        string query =
            @"MATCH (u:User)
            RETURN COUNT(u)";

        int response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<int>(query);

        return response;
    }
}
