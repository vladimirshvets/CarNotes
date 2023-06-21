using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;

namespace CarNotes.Persistence.Neo4j.Repositories;

public class CarRepository : ICarRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarRepository"/> class.
    /// </summary>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public CarRepository(INeo4jDataAccess neo4jDataAccess)
    {
        _neo4jDataAccess = neo4jDataAccess;
    }

    public async Task<IEnumerable<Car>> GetListAsync(Guid userId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car)
            RETURN c
            ORDER BY c.created_at DESC";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "c" }, parameters);

        List<Car> cars = new(response.Count);
        foreach (Dictionary<string, object> carObj in response)
        {
            Car car = new(carObj);
            cars.Add(car);
        }

        return cars;
    }

    public async Task<Car?> GetAsync(Guid userId, Guid carId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car { id: $carId })
            RETURN c";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() },
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "c" }, parameters);

        if (response.Count == 0)
        {
            return null;
        }

        return new Car(response[0]);
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

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() },
            { "make", car.Make },
            { "model", car.Model },
            { "generation", car.Generation },
            { "VIN", car.VIN },
            { "year", car.Year },
            { "plate", car.Plate },
            { "engineType", car.EngineType },
            { "ownedFrom", car.OwnedFrom },
            { "ownedTo", car.OwnedTo }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
            query, parameters);

        return new Car(response);
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

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "make", car.Make },
            { "model", car.Model },
            { "generation", car.Generation },
            { "VIN", car.VIN },
            { "year", car.Year },
            { "plate", car.Plate },
            { "engineType", car.EngineType != null ? car.EngineTypeText : null },
            { "ownedFrom", car.OwnedFrom },
            { "ownedTo", car.OwnedTo }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
                query, parameters);

        return new Car(response);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid carId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car { id: $carId })-[]->(anynode)
            DETACH DELETE c, anynode
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() },
            { "carId", carId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }

    public async Task<int> GetNumberOfCarsAsync()
    {
        string query =
            @"MATCH (c:Car)
            RETURN COUNT(c)";

        int response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<int>(query);

        return response;
    }
}
