using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class ServiceRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<ServiceRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRepository"/> class.
    /// </summary>
    public ServiceRepository(
        INeo4jDataAccess neo4jDataAccess,
        ILogger<ServiceRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of service records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of service records.</returns>
    public async Task<IEnumerable<Service>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER]-(s:Service)
            RETURN s, m
            ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "s", "m" }, parameters);

        int servicesCount = response.Count / 2;
        List<Service> services = new(servicesCount);
        for (int i = 0; i < servicesCount; i++)
        {
            Service service = new(response[i * 2])
            {
                Mileage = new Mileage(response[i * 2 + 1])
            };
            services.Add(service);
        }

        return services;
    }

    /// <summary>
    /// Creates a new service record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="service">Service data</param>
    /// <returns>A newly created instance of service.</returns>
    public async Task<Service> AddAsync(Guid carId, Guid mileageId, Service service)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (s:Service {
                    id: apoc.create.uuid(),
                    title: $title,
                    station_name: $stationName,
                    address: $address,
                    website_url: $websiteUrl,
                    cost_of_work: $costOfWork,
                    cost_of_spare_parts: $costOfSpareParts,
                    comment: $comment
                }),
                (c)-[:SERVICE { created_at: timestamp() }]->(s),
                (s)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN s, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "title", service.Title },
            { "stationName", service.StationName },
            { "address", service.Address },
            { "websiteUrl", service.WebsiteUrl },
            { "costOfWork", service.CostOfWork },
            { "costOfSpareParts", service.CostOfSpareParts },
            { "comment", service.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        Service newInstance = new(response[0])
        {
            Mileage = new Mileage(response[1])
        };

        return newInstance;
    }
}
