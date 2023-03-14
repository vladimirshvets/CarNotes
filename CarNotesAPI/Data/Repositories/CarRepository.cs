using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Repositories;

public class CarRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<CarRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarRepository"/> class.
    /// </summary>
    public CarRepository(
        INeo4jDataAccess neo4jDataAccess, ILogger<CarRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of cars of a specified user.
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Collection of cars.</returns>
    public async Task<IEnumerable<Car>> GetByUser(Guid userId)
    {
        string query =
            @"MATCH (u:User { id: $userId })-[rel: OWNS]->(c: Car)
            RETURN c";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() }
        };

        List<Dictionary<string, object>> response =
            await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, "c", parameters);

        List<Car> cars = new(response.Count);
        foreach (Dictionary<string, object> carObj in response)
        {
            Car car = PopulateFrom(carObj);
            cars.Add(car);
        }

        return cars;
    }

    /// <summary>
    /// Creates a new car record.
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="car">Car data</param>
    /// <returns>A newly created instance of car.</returns>
    public async Task<bool> AddAsync(Guid userId, Car car)
    {
        string query =
            @"MATCH (u: User { id: $userId })
            CREATE
                (c: Car {
                    id: apoc.create.uuid(),
                    make: $make,
                    model: $model,
                    generation: $generation,
                    VIN: $VIN,
                    year: $year,
                    plate: $plate
                }),
                (u)-[rel:OWNS { created_at: timestamp() }]->(c)
            RETURN TRUE";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() },
            { "make", car.Make },
            { "model", car.Model },
            { "generation", car.Generation },
            { "VIN", car.VIN },
            { "year", car.Year },
            { "plate", car.Plate }
        };

        return await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(
            query, parameters);
    }

    /// <summary>
    /// Updates an existing car record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="car">Updated car data</param>
    /// <returns>Updated instance of car.</returns>
    public async Task<Car> UpdateAsync(Guid carId, Car car)
    {
        string query =
            @"MATCH (c: Car { id: $carId })
            SET
                c.make = $make,
                c.model = $model,
                c.generation = $generation,
                c.VIN = $VIN,
                c.year = $year,
                c.plate = $plate,
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
            { "plate", car.Plate }
        };

        var carObj = await _neo4jDataAccess.ExecuteWriteTransactionAsync<Dictionary<string, object>>(
                query, parameters);

        return PopulateFrom(carObj);
    }

    /// <summary>
    /// Populates a car from the set of fields.
    /// </summary>
    /// <param name="carObj">Set of property names and their values</param>
    /// <returns>A new instance of car.</returns>
    private Car PopulateFrom(Dictionary<string, object> carObj)
    {
        Car car = new()
        {
            Id = new Guid((string)carObj["id"]),
            Make = carObj.ContainsKey("make") ? (string)carObj["make"] : string.Empty,
            Model = carObj.ContainsKey("model") ? (string)carObj["model"] : string.Empty,
            Generation = carObj.ContainsKey("generation") ? (string)carObj["generation"] : null,
            VIN = carObj.ContainsKey("VIN") ? (string)carObj["VIN"] : null,
            Year = carObj.ContainsKey("year") ? (int)(long)carObj["year"] : null,
            Plate = carObj.ContainsKey("plate") ? (string)carObj["plate"] : null
        };

        return car;
    }
}
