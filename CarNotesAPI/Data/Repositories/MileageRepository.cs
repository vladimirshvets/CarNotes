using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using Neo4j.Driver;

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
            ORDER BY m.Value DESC, m.Date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        List<Dictionary<string, object>> response =
            await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, "m", parameters);

        List<Mileage> mileages = new(response.Count);
        foreach (Dictionary<string, object> mileageObj in response)
        {
            Mileage mileage = PopulateFrom(mileageObj);
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
                    date: $mileageDate,
                    value: $mileageValue,
                    comment: $mileageComment
                }),
                (c)-[rel:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileage.Date },
            { "mileageValue", mileage.Value },
            { "mileageComment", mileage.Comment }
        };

        var mileageObj = await _neo4jDataAccess.ExecuteWriteTransactionAsync<Dictionary<string, object>>(
                query, parameters);

        return PopulateFrom(mileageObj);
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
                m.value = $updatedMileageValue,
                m.comment = $updatedMileageComment,
                rel.updated_at = timestamp()
            RETURN m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageDate", mileageDate },
            { "mileageValue", mileageValue },
            { "updatedMileageDate", mileage.Date },
            { "updatedMileageValue", mileage.Value },
            { "updatedMileageComment", mileage.Comment }
        };

        var mileageObj = await _neo4jDataAccess.ExecuteWriteTransactionAsync<Dictionary<string, object>>(
                query, parameters);

        return PopulateFrom(mileageObj);
    }

    /// <summary>
    /// Populates a mileage from the set of fields.
    /// </summary>
    /// <param name="mileageObj">Set of property names and their values</param>
    /// <returns>A new instance of mileage.</returns>
    private Mileage PopulateFrom(Dictionary<string, object> mileageObj)
    {
        Mileage mileage = new()
        {
            Date = ((LocalDate)mileageObj["date"]).ToDateOnly(),
            Value = (int)(long)mileageObj["value"],
            Comment = mileageObj.ContainsKey("Comment") ? (string)mileageObj["comment"] : null
        };

        return mileage;
    }
}
