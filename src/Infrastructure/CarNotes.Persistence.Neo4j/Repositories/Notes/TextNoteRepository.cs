using AutoMapper;
using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;

namespace CarNotes.Persistence.Neo4j.Repositories.Notes;

public class TextNoteRepository : INoteRepository<TextNote>
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextNoteRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public TextNoteRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    /// <summary>
    /// Returns a list of text note records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of text note records.</returns>
    public async Task<IEnumerable<TextNote>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER]-(t:TextNote)
            RETURN t, m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "t", "m" }, parameters);

        int textNotesCount = response.Count / 2;
        List<TextNote> textNotes = new(textNotesCount);
        for (int i = 0; i < textNotesCount; i++)
        {
            TextNote textNote = _mapper.Map<TextNote>(
                response[i * 2],
                opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[i * 2 + 1]));
            textNotes.Add(textNote);
        }

        return textNotes;
    }

    /// <summary>
    /// Creates a new text note record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="textNote">Text note data</param>
    /// <returns>A newly created instance of text note.</returns>
    public async Task<TextNote> AddAsync(Guid carId, Guid mileageId, TextNote textNote)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (t:TextNote {
                    id: apoc.create.uuid(),
                    title: $title,
                    tag: $tag,
                    text: $text,
                    comment: $comment
                }),
                (c)-[:TEXT { created_at: timestamp() }]->(t),
                (t)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN t, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "title", textNote.Title },
            { "tag", textNote.Tag },
            { "text", textNote.Text },
            { "comment", textNote.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        TextNote newInstance = _mapper.Map<TextNote>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return newInstance;
    }

    /// <summary>
    /// Updates an existing text note record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="textNoteId">Text note identifier</param>
    /// <param name="textNote">Text note data</param>
    /// <returns>An updated instance of text note.</returns>
    public async Task<TextNote> UpdateAsync(
        Guid carId, Guid mileageId, Guid textNoteId, TextNote textNote)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(t:TextNote { id: $textNoteId })
            SET
                t.title = $title,
                t.tag = $tag,
                t.text = $text,
                t.comment = $comment
            RETURN t, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "textNoteId", textNoteId.ToString() },
            { "title", textNote.Title },
            { "tag", textNote.Tag },
            { "text", textNote.Text },
            { "comment", textNote.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        TextNote updatedInstance = _mapper.Map<TextNote>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return updatedInstance;
    }

    /// <summary>
    /// Deletes an existing text note record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="textNoteId">Text note identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid textNoteId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(t:TextNote { id: $textNoteId })
            DETACH DELETE t
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "textNoteId", textNoteId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }
}
