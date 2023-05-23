using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    public UserRepository(INeo4jDataAccess neo4jDataAccess)
    {
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string query =
            @"MATCH (u:User { email: $email })
            RETURN u";

        var parameters = new Dictionary<string, object>
        {
            { "email", email }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "u" }, parameters);

        if (response.Count == 0)
        {
            return null;
        }

        return new User(response[0]);
    }
}
