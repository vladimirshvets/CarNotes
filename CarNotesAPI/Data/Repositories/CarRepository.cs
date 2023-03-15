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
            @"MATCH (u:User { id: $userId })-[rel:OWNS]->(c:Car)
            RETURN c";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "c" }, parameters);

        List<Car> cars = new(response.Count);
        foreach (Dictionary<string, object> carObj in response)
        {
            Car car = Car.FromNode(carObj);
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
                    plate: $plate
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
            { "plate", car.Plate }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
            query, parameters);

        return Car.FromNode(response);
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
            @"MATCH (c:Car { id: $carId })
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

        var response = await _neo4jDataAccess.ExecuteWriteWithDictionaryResultAsync(
                query, parameters);

        return Car.FromNode(response);
    }
}
