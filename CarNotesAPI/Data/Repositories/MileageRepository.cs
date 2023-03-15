﻿using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Repositories;

public class MileageRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<MileageRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MileageRepository"/> class.
    /// </summary>
    public MileageRepository(
        INeo4jDataAccess neo4jDataAccess, ILogger<MileageRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of mileage records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of mileage records.</returns>
    public async Task<IEnumerable<Mileage>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[rel: MILE_MARKER]->(m: Mileage)
            RETURN m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, "m", parameters);

        List<Mileage> mileages = new(response.Count);
        foreach (Dictionary<string, object> mileageObj in response)
        {
            Mileage mileage = Mileage.FromNode(mileageObj);
            mileages.Add(mileage);
        }

        return mileages;
    }

    /// <summary>
    /// Creates a new mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileage">Mileage data</param>
    /// <returns>A newly created instance of mileage.</returns>
    public async Task<Mileage> AddAsync(Guid carId, Mileage mileage)
    {
        string query =
            @"MATCH (c: Car { id: $carId })
            CREATE
                (m: Mileage {
                    id: apoc.create.uuid(),
                    date: $mileageDate,
                    odometer: $mileageValue
                }),
                (c)-[rel:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileage.Date },
            { "mileageValue", mileage.OdometerValue }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
                query, parameters);

        return Mileage.FromNode(response);
    }

    /// <summary>
    /// Updates an existing mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageDate">Current mileage date</param>
    /// <param name="mileageValue">Current mileage value</param>
    /// <param name="mileage">Updated mileage data</param>
    /// <returns>Updated instance of mileage.</returns>
    public async Task<Mileage> UpdateAsync(
        Guid carId, DateOnly mileageDate, long mileageValue, Mileage mileage)
    {
        string query =
            @"MATCH (:Car { id: $carId })-[rel:MILE_MARKER]->(m: Mileage { date: $mileageDate, value: $mileageValue })
            SET
                m.date = $updatedMileageDate,
                m.odometer = $updatedMileageValue,
                rel.updated_at = timestamp()
            RETURN m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileageDate },
            { "mileageValue", mileageValue },
            { "updatedMileageDate", mileage.Date },
            { "updatedMileageValue", mileage.OdometerValue }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
                query, parameters);

        return Mileage.FromNode(response);
    }
}
