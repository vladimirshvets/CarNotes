using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class WashingRepository : INoteRepository<Washing>
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="WashingRepository"/> class.
    /// </summary>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public WashingRepository(INeo4jDataAccess neo4jDataAccess)
    {
        _neo4jDataAccess = neo4jDataAccess;
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
            Washing washing = new(response[i * 2])
            {
                Mileage = new Mileage(response[i * 2 + 1])
            };
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
                    is_interior_cleaned: $isInteriorCleaned,
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
            { "isInteriorCleaned", washing.IsInteriorCleaned },
            { "totalAmount", washing.TotalAmount },
            { "comment", washing.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        Washing newInstance = new(response[0])
        {
            Mileage = new Mileage(response[1])
        };

        return newInstance;
    }

    /// <summary>
    /// Updates an existing washing record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="washingId">Washing identifier</param>
    /// <param name="washing">Washing data</param>
    /// <returns>An updated instance of washing.</returns>
    public async Task<Washing> UpdateAsync(
        Guid carId, Guid mileageId, Guid washingId, Washing washing)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(w:Washing { id: $washingId })
            SET
                w.title = $title,
                w.address = $address,
                w.is_contact = $isContact,
                w.is_degreaser_used = $isDegreaserUsed,
                w.is_polish_used = $isPolishUsed,
                w.is_antirain_used = $isAntiRainUsed,
                w.is_interior_cleaned = $isInteriorCleaned,
                w.total_amount = $totalAmount,
                w.comment = $comment
            RETURN w, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "washingId", washingId.ToString() },
            { "title", washing.Title },
            { "address", washing.Address },
            { "isContact", washing.IsContact },
            { "isDegreaserUsed", washing.IsDegreaserUsed },
            { "isPolishUsed", washing.IsPolishUsed },
            { "isAntiRainUsed", washing.IsAntiRainUsed },
            { "isInteriorCleaned", washing.IsInteriorCleaned },
            { "totalAmount", washing.TotalAmount },
            { "comment", washing.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        Washing updatedInstance = new(response[0])
        {
            Mileage = new Mileage(response[1])
        };

        return updatedInstance;
    }

    /// <summary>
    /// Deletes an existing washing record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="washingId">Washing identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid washingId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(w:Washing { id: $washingId })
            DETACH DELETE w
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "washingId", washingId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }
}
