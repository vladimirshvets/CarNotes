using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Models.Exceptions;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace CarNotesAPI.Data;

public class Neo4jDataAccess : INeo4jDataAccess
{
    private readonly IAsyncSession _session;

    private readonly ILogger<Neo4jDataAccess> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Neo4jDataAccess"/> class.
    /// </summary>
    public Neo4jDataAccess(
        IDriver driver,
        ILogger<Neo4jDataAccess> logger,
        IOptions<ApplicationSettings> appSettingsOptions)
    {
        _logger = logger;
        string database = appSettingsOptions.Value.Neo4jDatabase ?? "neo4j";
        _session = driver.AsyncSession(o => o.WithDatabase(database));
    }

    /// <summary>
    /// Execute read list as an asynchronous operation.
    /// </summary>
    public async Task<List<string>> ExecuteReadListAsync(
        string query,
        List<string> returnObjectKeys,
        IDictionary<string, object>? parameters = null)
    {
        return await ExecuteReadTransactionAsync<string>(
            query, returnObjectKeys, parameters);
    }

    /// <summary>
    /// Execute read dictionary as an asynchronous operation.
    /// </summary>
    public async Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(
        string query,
        List<string> returnObjectKeys,
        IDictionary<string, object>? parameters = null)
    {
        return await ExecuteReadTransactionAsync<Dictionary<string, object>>(
            query, returnObjectKeys, parameters);
    }

    /// <summary>
    /// Execute read scalar as an asynchronous operation.
    /// </summary>
    public async Task<T> ExecuteReadScalarAsync<T>(
        string query,
        IDictionary<string, object>? parameters = null)
    {
        try
        {
            parameters ??= new Dictionary<string, object>();

            var result = await _session.ExecuteReadAsync(async tx =>
            {
                T? scalar = default;

                var res = await tx.RunAsync(query, parameters);

                scalar = (await res.SingleAsync())[0].As<T>();

                return scalar;
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was a problem while executing database query");
            throw;
        }
    }

    /// <summary>
    /// Execute write transaction with a dictionary result
    /// as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Primitive type (struct)</typeparam>
    /// <param name="query">Query string</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Dictionary representation of result.</returns>
    /// <exception cref="MatchNotFoundException">
    /// Throws if no node matches the specified parameters.
    /// </exception>
    public async Task<T> ExecuteWriteWithScalarResultAsync<T>(
        string query,
        IDictionary<string, object>? parameters = null) where T : struct
    {
        var response = await ExecuteWriteTransactionAsync(query, parameters);
        if (response == null)
        {
            throw new MatchNotFoundException();
        }

        // Return the value of a primitive type.
        var value = response.First().Value;
        var scalar = value.As<T>();

        return scalar;
    }

    /// <summary>
    /// Execute write transaction with a dictionary result
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="query">Query string</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Dictionary representation of result.</returns>
    /// <exception cref="MatchNotFoundException">
    /// Throws if no node matches the specified parameters.
    /// </exception>
    public async Task<Dictionary<string, object>> ExecuteWriteWithDictionaryResultAsync(
        string query,
        IDictionary<string, object>? parameters = null)
    {
        var response = await ExecuteWriteTransactionAsync(query, parameters);
        if (response == null)
        {
            throw new MatchNotFoundException();
        }

        // Return the value of a class.
        var value = response.First().Value;
        var obj = (Dictionary<string, object>)value.As<INode>().Properties;

        return obj;
    }

    /// <summary>
    /// Execute write transaction with a list result
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="query">Query string</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>List representation of result.</returns>
    /// /// <exception cref="MatchNotFoundException">
    /// Throws if no node matches the specified parameters.
    /// </exception>
    public async Task<List<Dictionary<string, object>>> ExecuteWriteWithListResultAsync(
        string query,
        IDictionary<string, object>? parameters = null)
    {
        var response = await ExecuteWriteTransactionAsync(query, parameters);
        if (response == null)
        {
            throw new MatchNotFoundException();
        }

        // Return the collection of class values.
        var collection = new List<Dictionary<string, object>>();

        foreach (var item in response)
        {
            var value = item.Value;
            var obj = (Dictionary<string, object>)value.As<INode>().Properties;
            collection.Add(obj);
        }

        return collection;
    }

    /// <summary>
    /// Execute write transaction.
    /// </summary>
    private async Task<IReadOnlyDictionary<string, object>?> ExecuteWriteTransactionAsync(
        string query,
        IDictionary<string, object>? parameters = null)
    {
        try
        {
            parameters ??= new Dictionary<string, object>();

            var result = await _session.ExecuteWriteAsync(async tx =>
            {
                // This function can process batch of statements per once.
                var res = await tx.RunAsync(query, parameters);

                List<IRecord> records = await res.ToListAsync();
                if (records.Count == 0)
                {
                    return null;
                }

                // However, let's assume that one query is executed at a time
                // to keep the code maintainable.
                IRecord record = records[0];

                return record.Values;
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was a problem while executing database query");
            throw;
        }
    }

    /// <summary>
    /// Execute read transaction as an asynchronous operation.
    /// </summary>
    private async Task<List<T>> ExecuteReadTransactionAsync<T>(
        string query,
        List<string> returnObjectKeys,
        IDictionary<string, object>? parameters)
    {
        try
        {
            parameters ??= new Dictionary<string, object>();

            var result = await _session.ExecuteReadAsync(async tx =>
            {
                var data = new List<T>();

                var res = await tx.RunAsync(query, parameters);

                List<IRecord> records = await res.ToListAsync();

                // Single object key.
                //data = records
                //    .Select(x => (T)x.Values[returnObjectKey].As<INode>().Properties)
                //    .ToList();

                // Multiple object keys.
                data = (
                    from recordValue in records.SelectMany(r => r.Values)
                    where returnObjectKeys.Contains(recordValue.Key)
                    select (T)recordValue.Value.As<INode>().Properties
                ).ToList();

                return data;
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was a problem while executing database query");
            throw;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources asynchronously.
    /// </summary>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _session.CloseAsync();
    }
}
