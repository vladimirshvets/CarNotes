using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class CarRepository : ICarRepository
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public CarRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<IEnumerable<Car>> GetListAsync(Guid userId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car)
            RETURN c
            ORDER BY c.created_at DESC";

        var parameters = new Dictionary<string, object?>
        {
            { "userId", userId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        List<Car> cars = new(response.Count());
        foreach (IRecord record in response)
        {
            INode carNode = record.Values["c"].As<INode>();
            Car car = _mapper.Map<Car>(carNode);
            cars.Add(car);
        }

        return cars;
    }

    public async Task<Car?> GetAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })
            RETURN c";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        if (!record.Values.Any())
        {
            return null;
        }
        INode carNode = record.Values["c"].As<INode>();

        return _mapper.Map<Car>(carNode);
    }

    public async Task<Car> AddAsync(Guid userId, Car car)
    {
        string query =
            @"MATCH (u:User { id: $userId })
            CREATE
                (c:Car {
                    id: apoc.create.uuid(),
                    make: $make,
                    model: $model,
                    generation: $generation,
                    VIN: $VIN,
                    year: $year,
                    plate: $plate,
                    engine_type: $engineType,
                    owned_from: $ownedFrom,
                    owned_to: $ownedTo
                }),
                (u)-[rel:OWNS { created_at: timestamp() }]->(c)
            RETURN c";

        var parameters = new Dictionary<string, object?>
        {
            { "userId", userId.ToString() },
            { "make", car.Make },
            { "model", car.Model },
            { "generation", car.Generation },
            { "VIN", car.VIN },
            { "year", car.Year },
            { "plate", car.Plate },
            { "engineType", car.EngineTypeText },
            { "ownedFrom", car.OwnedFrom },
            { "ownedTo", car.OwnedTo }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode carNode = record.Values["c"].As<INode>();

        return _mapper.Map<Car>(carNode);
    }

    public async Task<Car> UpdateAsync(Guid carId, Car car)
    {
        string query =
            @"MATCH (c:Car { id: $carId })
            SET
                c.make = $make,
                c.model = $model,
                c.generation = $generation,
                c.VIN = $VIN,
                c.year = $year,
                c.plate = $plate,
                c.engine_type = $engineType,
                c.owned_from = $ownedFrom,
                c.owned_to = $ownedTo,
                c.updated_at = timestamp()
            RETURN c";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "make", car.Make },
            { "model", car.Model },
            { "generation", car.Generation },
            { "VIN", car.VIN },
            { "year", car.Year },
            { "plate", car.Plate },
            { "engineType", car.EngineTypeText },
            { "ownedFrom", car.OwnedFrom },
            { "ownedTo", car.OwnedTo }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode carNode = record.Values["c"].As<INode>();

        return _mapper.Map<Car>(carNode);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid carId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car { id: $carId })
            OPTIONAL MATCH (c)-->(note)
            DETACH DELETE c, note
            RETURN true";

        var parameters = new Dictionary<string, object?>
        {
            { "userId", userId.ToString() },
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        bool result = record[0].As<bool>();

        return result;
    }

    public async Task<int> GetNumberOfCarsAsync()
    {
        string query =
            @"MATCH (c:Car)
            RETURN COUNT(c)";

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(query);

        IRecord record = response.First();
        int result = record[0].As<int>();

        return result;
    }

    public async Task<bool> SetAvatarUrlAsync(Guid carId, string url)
    {
        string query =
            @"MATCH (c:Car { id: $carId })
            SET
                c.avatar_url = $avatarUrl,
                c.updated_at = timestamp()
            RETURN true";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "avatarUrl", url }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        bool result = record[0].As<bool>();

        return result;
    }
}
