using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j;

public class Neo4jDataAccess : INeo4jDataAccess
{
    const string Neo4jDefaultDatabase = "neo4j";

    private readonly string _database;

    private readonly IDriver _driver;

    private readonly ILogger<Neo4jDataAccess> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Neo4jDataAccess"/> class.
    /// </summary>
    public Neo4jDataAccess(
        IDriver driver,
        ILogger<Neo4jDataAccess> logger,
        Neo4jOptions neo4JOptions)
    {
        _driver = driver;
        _logger = logger;
        _database = neo4JOptions.Database ?? Neo4jDefaultDatabase;
    }

    /// <summary>
    /// Execute read transaction.
    /// </summary>
    /// <param name="query">Query string</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Collection that represents the result.</returns>
    public async Task<IEnumerable<IRecord>> ExecuteReadTransactionAsync(
        string query,
        IDictionary<string, object?>? parameters = null)
    {
        parameters ??= new Dictionary<string, object?>();

        try
        {
            using var session =
                _driver.AsyncSession(o => o.WithDatabase(_database));

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var res = await tx.RunAsync(query, parameters);
                List<IRecord> records = await res.ToListAsync();

                return records;
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was a problem while executing database query.");
            throw;
        }
    }

    /// <summary>
    /// Execute write transaction.
    /// </summary>
    /// <param name="query">Query string</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Collection that represents the result.</returns>
    public async Task<IEnumerable<IRecord>> ExecuteWriteTransactionAsync(
        string query,
        IDictionary<string, object?>? parameters)
    {
        parameters ??= new Dictionary<string, object?>();

        try
        {
            using var session =
                _driver.AsyncSession(o => o.WithDatabase(_database));

            var result = await session.ExecuteWriteAsync(async tx =>
            {
                var res = await tx.RunAsync(query, parameters);
                List<IRecord> records = await res.ToListAsync();

                return records;
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was a problem while executing database query.");
            throw;
        }
    }
}
