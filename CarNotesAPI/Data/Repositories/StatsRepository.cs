﻿using CarNotesAPI.Data.Api;

namespace CarNotesAPI.Data.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatsRepository"/> class.
    /// </summary>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public StatsRepository(INeo4jDataAccess neo4jDataAccess)
    {
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<int> GetNumberOfRecordsAsync(
        Guid carId, IEnumerable<string> relationTypes)
    {
        string relationTypesString = string.Join('|', relationTypes).Trim();
        if (relationTypesString.Length == 0)
        {
            return 0;
        }

        string query =
            @"MATCH (:Car { id: $carId })-[rel:" + relationTypesString + @"]->()
            RETURN COUNT(rel)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "relTypes", relationTypesString }
        };

        int response = await _neo4jDataAccess.ExecuteReadScalarAsync<int>(
            query, parameters);
        return response;
    }

    public async Task<double> GetTotalFuelConsumed(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:FUEL]->(r:Refueling)
            RETURN SUM(r.volume)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        double response = await _neo4jDataAccess.ExecuteReadScalarAsync<double>(
            query, parameters);

        return response;
    }
}