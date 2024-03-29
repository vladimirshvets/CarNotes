﻿using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatsRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public StatsRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<int> GetNumberOfRecordsAsync(
        Guid carId, IEnumerable<string> relationTypes)
    {
        string relationTypesString = GetTypesAsString(relationTypes);
        if (relationTypesString.Length == 0)
        {
            return 0;
        }

        string query =
            @"MATCH (:Car { id: $carId })-[rel:" + relationTypesString + @"]->()
            RETURN COUNT(rel)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        int result = record[0].As<int>();

        return result;
    }

    public async Task<double> GetTotalFuelConsumedAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:FUEL]->(r:Refueling)
            RETURN SUM(r.volume)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        double result = record[0].As<double>();

        return result;
    }

    public async Task<double> GetTotalMoneySpentAsync(
        Guid carId, IEnumerable<string> noteTypes)
    {
        string noteTypesString = GetTypesAsString(noteTypes);
        if (noteTypesString.Length == 0)
        {
            return 0;
        }

        string query =
            @"MATCH (c:Car { id: $carId })-->(n:" + noteTypesString + @")
            RETURN SUM(n.total_amount)";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        double result = record[0].As<double>();

        return result;
    }

    public async Task<IEnumerable<Mileage>> GetMileagesWithRelatedNotesAsync(
        Guid carId,
        IEnumerable<string> noteTypes,
        int skip,
        int take)
    {
        string noteTypesString = GetTypesAsString(noteTypes);
        if (noteTypesString.Length == 0)
        {
            return Enumerable.Empty<Mileage>();
        }

        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)
            OPTIONAL MATCH (n:" + noteTypesString + @")-->(m)
            RETURN m, collect(n) as notes
            ORDER BY m.odometer DESC, m.date DESC
            SKIP $skip LIMIT $take";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "skip", skip },
            { "take", take }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        /*
         * Parse the response as a list of mileages with linked notes.
         */
        var mileages = new List<Mileage>();
        foreach (IRecord record in response)
        {
            INode mileageNode = record.Values["m"].As<INode>();
            Mileage mileage = _mapper.Map<Mileage>(mileageNode);

            var notes = new List<Note>();
            var noteNodeCollection = record.Values["notes"].As<IEnumerable<INode>>();
            foreach (INode node in noteNodeCollection)
            {
                string noteTypeName = node.Labels[0];
                Note note = MakeNoteByType(noteTypeName, node);
                notes.Add(note);
            }
            mileage.Notes = notes;
            mileages.Add(mileage);
        }

        return mileages;
    }

    /// <summary>
    /// Converts the collection of node or relation types to the query substring.
    /// </summary>
    /// <param name="types">Collection of node or relation types</param>
    /// <returns>Query substring.</returns>
    private string GetTypesAsString(IEnumerable<string> types)
        => string.Join('|', types).Trim();

    /// <summary>
    /// Make note of a specified type from dictionary.
    /// </summary>
    /// <param name="noteType">Note type.</param>
    /// <param name="source">Data source.</param>
    /// <returns>Instance of specified type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws if the specified type is not derived from the <see cref="Note"/>.
    /// </exception>
    private Note MakeNoteByType(
        string noteType, INode source)
    {
        // ToDo:
        // Try to convert note type name to Type variable
        // and pass it to the Map<T>() as T.
        // Otherwise, it will be necessary to add new note types every time.
        return noteType switch
        {
            nameof(LegalProcedure) => _mapper.Map<LegalProcedure>(
                source, opt => opt.Items["Mileage"] = null),

            nameof(Refueling) => _mapper.Map<Refueling>(
                source, opt => opt.Items["Mileage"] = null),

            nameof(Service) => _mapper.Map<Service>(
                source, opt => opt.Items["Mileage"] = null),

            nameof(SparePart) => _mapper.Map<SparePart>(
                source, opt => opt.Items["Mileage"] = null),

            nameof(TextNote) => _mapper.Map<TextNote>(
                source, opt => opt.Items["Mileage"] = null),

            nameof(Washing) => _mapper.Map<Washing>(
                source, opt => opt.Items["Mileage"] = null),

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
