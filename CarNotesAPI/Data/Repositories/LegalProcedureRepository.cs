using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class LegalProcedureRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<LegalProcedureRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LegalProcedureRepository"/> class.
    /// </summary>
    public LegalProcedureRepository(
        INeo4jDataAccess neo4jDataAccess,
        ILogger<LegalProcedureRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
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
            LegalProcedure legalProcedure = new(response[i * 2])
            {
                Mileage = new Mileage(response[i * 2 + 1])
            };
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

        LegalProcedure newInstance = new(response[0])
        {
            Mileage = new Mileage(response[1])
        };

        return newInstance;
    }
}
