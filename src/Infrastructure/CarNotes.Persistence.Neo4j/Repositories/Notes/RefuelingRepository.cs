using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories.Notes;

public class RefuelingRepository : INoteRepository<Refueling>
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefuelingRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public RefuelingRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
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

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        List<Refueling> refuelings = new(response.Count());
        foreach (IRecord record in response)
        {
            INode refuelingNode = record.Values["r"].As<INode>();
            INode mileageNode = record.Values["m"].As<INode>();
            Refueling refueling = _mapper.Map<Refueling>(
                refuelingNode,
                opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));
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
                    total_amount: $totalAmount,
                    distributor: $distributor,
                    address: $address,
                    comment: $comment
                }),
                (c)-[:FUEL { created_at: timestamp() }]->(r),
                (r)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN r, m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "volume", refueling.Volume },
            { "price", refueling.Price },
            { "totalAmount", refueling.TotalAmount },
            { "distributor", refueling.Distributor },
            { "address", refueling.Address },
            { "comment", refueling.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode refuelingNode = record.Values["r"].As<INode>();
        INode mileageNode = record.Values["m"].As<INode>();

        Refueling newInstance = _mapper.Map<Refueling>(
            refuelingNode,
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));

        return newInstance;
    }

    /// <summary>
    /// Updates an existing refueling record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="refuelingId">Refueling identifier</param>
    /// <param name="refueling">Refueling data</param>
    /// <returns>An updated instance of refueling.</returns>
    public async Task<Refueling> UpdateAsync(
        Guid carId, Guid mileageId, Guid refuelingId, Refueling refueling)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(r:Refueling { id: $refuelingId })
            SET
                r.volume = $volume,
                r.price = $price,
                r.total_amount = $totalAmount,
                r.distributor = $distributor,
                r.address = $address,
                r.comment = $comment
            RETURN r, m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "refuelingId", refuelingId.ToString() },
            { "volume", refueling.Volume },
            { "price", refueling.Price },
            { "totalAmount", refueling.TotalAmount },
            { "distributor", refueling.Distributor },
            { "address", refueling.Address },
            { "comment", refueling.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode refuelingNode = record.Values["r"].As<INode>();
        INode mileageNode = record.Values["m"].As<INode>();

        Refueling updatedInstance = _mapper.Map<Refueling>(
            refuelingNode,
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));

        return updatedInstance;
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

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "refuelingId", refuelingId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        bool result = record[0].As<bool>();

        return result;
    }
}
