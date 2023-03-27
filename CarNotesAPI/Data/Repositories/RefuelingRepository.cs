using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class RefuelingRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<RefuelingRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefuelingRepository"/> class.
    /// </summary>
    public RefuelingRepository(
        INeo4jDataAccess neo4jDataAccess, ILogger<RefuelingRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of refueling records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of refueling records.</returns>
    public async Task<IEnumerable<Refueling>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER]-(r:Refueling)
            RETURN r, m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
            query, new List<string> { "r", "m" }, parameters);

        int refuelingsCount = response.Count / 2;
        List<Refueling> refuelings = new(refuelingsCount);
        for (int i = 0; i < refuelingsCount; i++)
        {
            Refueling refueling = new(response[i * 2])
            {
                Mileage = new Mileage(response[i * 2 + 1])
            };
            refuelings.Add(refueling);
        }

        return refuelings;
    }

    /// <summary>
    /// Creates a new refueling record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="refueling">Refueling data</param>
    /// <returns>A newly created instance of refueling.</returns>
    public async Task<Refueling> AddAsync(
        Guid carId, Guid mileageId, Refueling refueling)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (r:Refueling {
                    id: apoc.create.uuid(),
                    volume: $volume,
                    price: $price,
                    distributor: $distributor,
                    address: $address,
                    comment: $comment
                }),
                (c)-[:FUEL { created_at: timestamp() }]->(r),
                (r)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN r, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "volume", refueling.Volume },
            { "price", refueling.Price },
            { "distributor", refueling.Distributor },
            { "address", refueling.Address },
            { "comment", refueling.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        Refueling newInstance = new(response[0])
        {
            Mileage = new Mileage(response[1])
        };

        return newInstance;
    }

    /// <summary>
    /// Deletes an existing refueling record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="refuelingId">Refueling identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid refuelingId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(r:Refueling { id: $refuelingId })
            DETACH DELETE r
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "refuelingId", refuelingId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }
}
