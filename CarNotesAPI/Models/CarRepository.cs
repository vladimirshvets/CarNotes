using System.Collections.Generic;
using CarNotesAPI.Models.Api;

namespace CarNotesAPI.Models;

public class CarRepository
	{
    private INeo4jDataAccess _neo4jDataAccess;

    private ILogger<CarRepository> _logger;

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
    /// Searches the list of cars by username.
    /// </summary>
    public async Task<List<Car>>
        SearchCarsByUsername(string username)
    {
        string query =
            @"MATCH (u:User { username: $username })-[rel: OWNS]->(c: Car)
            RETURN c;";
        var parameters = new Dictionary<string, object>
        {
            { "username", username }
        };

        List<Dictionary<string, object>> response =
            await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, "c", parameters);

        List<Car> cars = new(response.Count);
        foreach (Dictionary<string, object> carObject in response)
        {
            Car car = new Car()
            {
                Make = carObject.ContainsKey("title") ? (string)carObject["title"] : string.Empty,
                Model = carObject.ContainsKey("title") ? (string)carObject["title"] : string.Empty,
                Generation = carObject.ContainsKey("generation") ? (string)carObject["generation"] : null,
                VIN = carObject.ContainsKey("VIN") ? (string)carObject["VIN"] : null,
                Year = carObject.ContainsKey("year") ? (int)carObject["year"] : null,
            };
            cars.Add(car);
        }

        return cars;
    }

    /// <summary>
    /// Adds a new car.
    /// </summary>
    public async Task<bool> Add(string username, Car car)
    {
        if (car != null)
        {
            string query =
                @"MATCH (u: User { username: '$username' })
                CREATE
                    (c: Car {
                        make: '$car.make',
                        model: '$car.model',
                        generation: '$car.generation',
                        VIN: '$car.VIN',
                        year: '$car.year'
                    }),
                    (u)-[rel:OWNS { from: $timestamp() }]->(c)
                RETURN true";

            var parameters = new Dictionary<string, object>
            {
                { "make", car.Make },
                { "model", car.Model },
                { "generation", car.Generation },
                { "VIN", car.VIN },
                { "year", car.Year }
            };
            return await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(
                query, parameters);
        }
        else
        {
            throw new ArgumentNullException(
                nameof(car), "Car must not be null");
        }
    }

    /// <summary>
    /// Get count of cars.
    /// </summary>
    public async Task<int> GetCarCount()
    {
        string query =
            @"MATCH (c:Car)
            RETURN COUNT(c) as carCount";
        int count = await _neo4jDataAccess.ExecuteReadScalarAsync<int>(query);
        return count;
    }

}
