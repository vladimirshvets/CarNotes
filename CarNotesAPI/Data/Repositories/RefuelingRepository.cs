using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;

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
    /// Creates a new refueling record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="refueling">Refueling data</param>
    /// <returns>A newly created instance of refueling.</returns>
    public async Task<Refueling> AddAsync(Guid carId, Guid mileageId, Refueling refueling)
    {
        string query =
            @"MATCH (c: Car { id: $carId })-[:MILE_MARKER]->(m: Mileage { id: $mileageId })
            CREATE
                (r: Refueling {
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

        Refueling newInstance = Refueling.FromNode(response[0]);
        newInstance.Mileage = Mileage.FromNode(response[1]);

        return newInstance;
    }
}
