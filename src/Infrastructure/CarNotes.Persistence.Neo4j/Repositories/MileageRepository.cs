using AutoMapper;
using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;

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

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
            query, new List<string> { "m" }, parameters);

        List<Mileage> mileages = new(response.Count);
        foreach (Dictionary<string, object> mileageObj in response)
        {
            Mileage mileage = _mapper.Map<Mileage>(mileageObj);
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

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileage.Date },
            { "mileageValue", mileage.OdometerValue }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
            query, parameters);

        return _mapper.Map<Mileage>(response);
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

        return _mapper.Map<Mileage>(response);
    }

    public async Task<bool> DeleteAsync(Guid carId, Guid mileageId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            DETACH DELETE m
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }

    public async Task<int> GetRelatedRecordsCountAsync(
        Guid carId, Guid mileageId)
    {
        string query =
            @"MATCH (:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[rel:MILE_MARKER]-()
            RETURN COUNT(rel)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() }
        };

        int response = await _neo4jDataAccess.ExecuteReadScalarAsync<int>(
            query, parameters);
        return response;
    }

    public async Task<int> GetMinOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MIN(m.odometer)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        int? response = await _neo4jDataAccess.ExecuteReadScalarAsync<int?>(
            query, parameters);

        return response ?? 0;
    }

    public async Task<int> GetMaxOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MAX(m.odometer)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        int? response = await _neo4jDataAccess.ExecuteReadScalarAsync<int?>(
            query, parameters);

        return response ?? 0;
    }

    public async Task<int> GetDeltaOdometerValueAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            RETURN MAX(m.odometer) - MIN(m.odometer)";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        int? response = await _neo4jDataAccess.ExecuteReadScalarAsync<int?>(
            query, parameters);

        return response ?? 0;
    }
}
