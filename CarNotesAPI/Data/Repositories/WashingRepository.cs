﻿using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class WashingRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<WashingRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WashingRepository"/> class.
    /// </summary>
    public WashingRepository(
        INeo4jDataAccess neo4jDataAccess,
        ILogger<WashingRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of washing records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of washing records.</returns>
    public async Task<IEnumerable<Washing>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER]-(w:Washing)
                RETURN w, m
                ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "w", "m" }, parameters);

        int washingsCount = response.Count / 2;
        List<Washing> washings = new(washingsCount);
        for (int i = 0; i < washingsCount; i++)
        {
            Washing washing = Washing.FromNode(response[i * 2]);
            washing.Mileage = Mileage.FromNode(response[i * 2 + 1]);
            washings.Add(washing);
        }

        return washings;
    }

    /// <summary>
    /// Creates a new washing record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="washing">Washing data</param>
    /// <returns>A newly created instance of washing.</returns>
    public async Task<Washing> AddAsync(Guid carId, Guid mileageId, Washing washing)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (w:Washing {
                    id: apoc.create.uuid(),
                    title: $title,
                    address: $address,
                    is_contact: $isContact,
                    is_degreaser_used: $isDegreaserUsed,
                    is_polish_used: $isPolishUsed,
                    is_antirain_used: $isAntiRainUsed,
                    total_amount: $totalAmount,
                    comment: $comment
                }),
                (c)-[:WASH { created_at: timestamp() }]->(w),
                (w)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN w, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "title", washing.Title },
            { "address", washing.Address },
            { "isContact", washing.IsContact },
            { "isDegreaserUsed", washing.IsDegreaserUsed },
            { "isPolishUsed", washing.IsPolishUsed },
            { "isAntiRainUsed", washing.IsAntiRainUsed },
            { "totalAmount", washing.TotalAmount },
            { "comment", washing.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        Washing newInstance = Washing.FromNode(response[0]);
        newInstance.Mileage = Mileage.FromNode(response[1]);

        return newInstance;
    }
}
