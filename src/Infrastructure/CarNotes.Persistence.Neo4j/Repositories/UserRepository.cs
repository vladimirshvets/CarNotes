using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public UserRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string query =
            @"MATCH (u:User { email: $email })
            RETURN u";

        var parameters = new Dictionary<string, object?>
        {
            { "email", email.ToLower() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        if (!record.Values.Any())
        {
            return null;
        }
        INode userNode = record.Values["u"].As<INode>();

        return _mapper.Map<User>(userNode);
    }

    public async Task<User?> GetAsync(Guid userId)
    {
        string query =
            @"MATCH (u:User { id: $userId })
            RETURN u";

        var parameters = new Dictionary<string, object?>
        {
            { "userId", userId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        if (!record.Values.Any())
        {
            return null;
        }
        INode userNode = record.Values["u"].As<INode>();

        return _mapper.Map<User>(userNode);
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

        var parameters = new Dictionary<string, object?>
        {
            { "userName", user.UserName },
            { "email", user.Email.ToLower() },
            { "passwordHash", user.PasswordHash },
            { "firstName", user.FirstName },
            { "lastName", user.LastName }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode userNode = record.Values["u"].As<INode>();

        return _mapper.Map<User>(userNode);
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

        var parameters = new Dictionary<string, object?>
        {
            { "userId", userId.ToString() },
            { "userName", user.UserName },
            { "email", user.Email },
            { "passwordHash", user.PasswordHash },
            { "firstName", user.FirstName },
            { "lastName", user.LastName }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode userNode = record.Values["u"].As<INode>();

        return _mapper.Map<User>(userNode);
    }

    public async Task<int> GetNumberOfUsersAsync()
    {
        string query =
            @"MATCH (u:User)
            RETURN COUNT(u)";

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(query);

        IRecord record = response.First();
        int result = record[0].As<int>();

        return result;
    }
}
