﻿using System;
using CarNotesAPI.Models.Api;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace CarNotesAPI.Models;

public class Neo4jDataAccess : INeo4jDataAccess
{
    private IAsyncSession _session;

    private ILogger<Neo4jDataAccess> _logger;

    private string _database;

    /// <summary>
    /// Initializes a new instance of the <see cref="Neo4jDataAccess"/> class.
    /// </summary>
    public Neo4jDataAccess(
        IDriver driver,
        ILogger<Neo4jDataAccess> logger,
        IOptions<ApplicationSettings> appSettingsOptions)
    {
        _logger = logger;
        _database = appSettingsOptions.Value.Neo4jDatabase ?? "neo4j";
        _session = driver.AsyncSession(o => o.WithDatabase(_database));
    }

    /// <summary>
    /// Execute read list as an asynchronous operation.
    /// </summary>
    public async Task<List<string>> ExecuteReadListAsync(
        string query,
        string returnObjectKey,
        IDictionary<string, object>? parameters = null)
    {
        return await ExecuteReadTransactionAsync<string>(
            query, returnObjectKey, parameters);
    }

    /// <summary>
    /// Execute read dictionary as an asynchronous operation.
    /// </summary>
    public async Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(
        string query,
        string returnObjectKey,
        IDictionary<string, object>?
        parameters = null)
    {
        return await ExecuteReadTransactionAsync<Dictionary<string, object>>(
            query, returnObjectKey, parameters);
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
            parameters = parameters == null ? new Dictionary<string, object>() : parameters;

            var result = await _session.ExecuteReadAsync(async tx =>
            {
                T scalar = default(T);

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
    /// Execute write transaction
    /// </summary>
    public async Task<T> ExecuteWriteTransactionAsync<T>(
        string query,
        IDictionary<string, object>? parameters = null)
    {
        try
        {
            parameters = parameters == null ? new Dictionary<string, object>() : parameters;

            var result = await _session.ExecuteWriteAsync(async tx =>
            {
                T scalar = default(T);

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
    /// Execute read transaction as an asynchronous operation.
    /// </summary>
    private async Task<List<T>> ExecuteReadTransactionAsync<T>(
        string query,
        string returnObjectKey,
        IDictionary<string, object>? parameters)
    {
        try
        {
            parameters = parameters ?? new Dictionary<string, object>();

            var result = await _session.ExecuteReadAsync(async tx =>
            {
                var data = new List<T>();

                var res = await tx.RunAsync(query, parameters);

                var records = await res.ToListAsync();

                data = records
                    .Select(x => (T)x.Values[returnObjectKey].As<INode>().Properties)
                    .ToList();

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
