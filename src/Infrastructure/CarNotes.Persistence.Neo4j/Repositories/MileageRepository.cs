using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class MileageRepository : IMileageRepository
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="MileageRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public MileageRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<IEnumerable<Mileage>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[rel:MILE_MARKER]->(m:Mileage)
            RETURN m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        List<Mileage> mileages = new(response.Count());
        foreach (IRecord record in response)
        {
            INode mileageNode = record.Values["m"].As<INode>();
            Mileage mileage = _mapper.Map<Mileage>(mileageNode);
            mileages.Add(mileage);
        }

        return mileages;
    }

    public async Task<Mileage> AddAsync(Guid carId, Mileage mileage)
    {
        string query =
            @"MATCH (c:Car { id: $carId })
            CREATE
                (m: Mileage {
                    id: apoc.create.uuid(),
                    date: $mileageDate,
                    odometer: $mileageValue
                }),
                (c)-[rel:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileage.Date },
            { "mileageValue", mileage.OdometerValue }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode mileageNode = record.Values["m"].As<INode>();

        return _mapper.Map<Mileage>(mileageNode);
    }

    public async Task<Mileage> UpdateAsync(
        Guid carId, DateOnly mileageDate, long mileageValue, Mileage mileage)
    {
        string query =
            @"MATCH (:Car { id: $carId })-[rel:MILE_MARKER]->(m:Mileage { date: $mileageDate, value: $mileageValue })
            SET
                m.date = $updatedMileageDate,
                m.odometer = $updatedMileageValue,
                rel.updated_at = timestamp()
            RETURN m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileageDate },
            { "mileageValue", mileageValue },
            { "updatedMileageDate", mileage.Date },
            { "updatedMileageValue", mileage.OdometerValue }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode mileageNode = record.Values["m"].As<INode>();

        return _mapper.Map<Mileage>(mileageNode);
    }

    public async Task<bool> DeleteAsync(Guid carId, Guid mileageId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            DETACH DELETE m
            RETURN true";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        bool result = record[0].As<bool>();

        return result;
    }

    public async Task<int> GetRelatedRecordsCountAsync(
        Guid carId, Guid mileageId)
    {
        string query =
            @"MATCH (:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[rel:MILE_MARKER]-()
            RETURN COUNT(rel)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        int result = record[0].As<int>();

        return result;
    }

    public async Task<int> GetMinOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MIN(m.odometer)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        int? result = record[0].As<int?>();

        return result ?? 0;
    }

    public async Task<int> GetMaxOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MAX(m.odometer)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        int? result = record[0].As<int?>();

        return result ?? 0;
    }

    public async Task<int> GetDeltaOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MAX(m.odometer) - MIN(m.odometer)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        int? result = record[0].As<int?>();

        return result ?? 0;
    }
}
