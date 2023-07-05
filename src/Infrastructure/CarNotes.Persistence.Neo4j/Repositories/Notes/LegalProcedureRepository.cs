using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;

namespace CarNotes.Persistence.Neo4j.Repositories.Notes;

public class LegalProcedureRepository : INoteRepository<LegalProcedure>
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LegalProcedureRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public LegalProcedureRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    /// <summary>
    /// Returns a list of legal procedure records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of legal procedure records.</returns>
    public async Task<IEnumerable<LegalProcedure>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER]-(l:LegalProcedure)
            RETURN l, m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
            query, new List<string> { "l", "m" }, parameters);

        int legalProceduresCount = response.Count / 2;
        List<LegalProcedure> legalProcedures = new(legalProceduresCount);
        for (int i = 0; i < legalProceduresCount; i++)
        {
            LegalProcedure legalProcedure = _mapper.Map<LegalProcedure>(
                response[i * 2],
                opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[i * 2 + 1]));
            legalProcedures.Add(legalProcedure);
        }

        return legalProcedures;
    }

    /// <summary>
    /// Creates a new legal procedure record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="legalProcedure">Legal procedure data</param>
    /// <returns>A newly created instance of legal procedure.</returns>
    public async Task<LegalProcedure> AddAsync(
        Guid carId, Guid mileageId, LegalProcedure legalProcedure)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (l:LegalProcedure {
                    id: apoc.create.uuid(),
                    title: $title,
                    group: $group,
                    total_amount: $totalAmount,
                    expiration_date: $expirationDate,
                    comment: $comment
                }),
                (c)-[:LEGAL { created_at: timestamp() }]->(l),
                (l)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN l, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "group", legalProcedure.Group },
            { "title", legalProcedure.Title },
            { "totalAmount", legalProcedure.TotalAmount },
            { "expirationDate", legalProcedure.ExpirationDate },
            { "comment", legalProcedure.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        LegalProcedure newInstance = _mapper.Map<LegalProcedure>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return newInstance;
    }

    /// <summary>
    /// Updates an existing legal procedure record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="legalProcedureId">Legal procedure identifier</param>
    /// <param name="legalProcedure">Legal procedure data</param>
    /// <returns>An updated instance of legal procedure.</returns>
    public async Task<LegalProcedure> UpdateAsync(
        Guid carId,
        Guid mileageId,
        Guid legalProcedureId,
        LegalProcedure legalProcedure)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(l:LegalProcedure { id: $legalProcedureId })
            SET
                l.title = $title,
                l.group = $group,
                l.total_amount = $totalAmount,
                l.expiration_date = $expirationDate,
                l.comment = $comment
            RETURN l, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "legalProcedureId", legalProcedureId.ToString() },
            { "title", legalProcedure.Title },
            { "group", legalProcedure.Group },
            { "totalAmount", legalProcedure.TotalAmount },
            { "expirationDate", legalProcedure.ExpirationDate },
            { "comment", legalProcedure.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        LegalProcedure updatedInstance = _mapper.Map<LegalProcedure>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return updatedInstance;
    }

    /// <summary>
    /// Deletes an existing legal procedure record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="legalProcedureId">Legal procedure identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid legalProcedureId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(l:LegalProcedure { id: $legalProcedureId })
            DETACH DELETE l
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "legalProcedureId", legalProcedureId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }
}
