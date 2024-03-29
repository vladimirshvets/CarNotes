﻿using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Repositories.Notes;

public class ServiceRepository : INoteRepository<Service>
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public ServiceRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
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

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteReadTransactionAsync(
            query, parameters);

        List<Service> services = new(response.Count());
        foreach (IRecord record in response)
        {
            INode serviceNode = record.Values["s"].As<INode>();
            INode mileageNode = record.Values["m"].As<INode>();
            Service service = _mapper.Map<Service>(
                serviceNode,
                opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));
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
                    total_amount: $totalAmount,
                    comment: $comment
                }),
                (c)-[:SERVICE { created_at: timestamp() }]->(s),
                (s)-[:MILE_MARKER { created_at: timestamp() }]->(m)
            RETURN s, m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "title", service.Title },
            { "stationName", service.StationName },
            { "address", service.Address },
            { "websiteUrl", service.WebsiteUrl },
            { "costOfWork", service.CostOfWork },
            { "costOfSpareParts", service.CostOfSpareParts },
            { "totalAmount", service.TotalAmount },
            { "comment", service.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode serviceNode = record.Values["s"].As<INode>();
        INode mileageNode = record.Values["m"].As<INode>();

        Service newInstance = _mapper.Map<Service>(
            serviceNode,
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));

        return newInstance;
    }

    /// <summary>
    /// Updates an existing service record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="serviceId">Service identifier</param>
    /// <param name="service">Service data</param>
    /// <returns>An updated instance of service.</returns>
    public async Task<Service> UpdateAsync(
        Guid carId, Guid mileageId, Guid serviceId, Service service)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(s:Service { id: $serviceId })
            SET
                s.title = $title,
                s.station_name = $stationName,
                s.address = $address,
                s.website_url = $websiteUrl,
                s.cost_of_work = $costOfWork,
                s.cost_of_spare_parts = $costOfSpareParts,
                s.total_amount = $totalAmount,
                s.comment = $comment
            RETURN s, m";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "serviceId", serviceId.ToString() },
            { "title", service.Title },
            { "stationName", service.StationName },
            { "address", service.Address },
            { "websiteUrl", service.WebsiteUrl },
            { "costOfWork", service.CostOfWork },
            { "costOfSpareParts", service.CostOfSpareParts },
            { "totalAmount", service.TotalAmount },
            { "comment", service.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        INode serviceNode = record.Values["s"].As<INode>();
        INode mileageNode = record.Values["m"].As<INode>();

        Service updatedInstance = _mapper.Map<Service>(
            serviceNode,
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(mileageNode));

        return updatedInstance;
    }

    /// <summary>
    /// Deletes an existing service record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="serviceId">Service identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid serviceId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(s:Service { id: $serviceId })
            DETACH DELETE s
            RETURN true";

        var parameters = new Dictionary<string, object?>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "serviceId", serviceId.ToString() }
        };

        var response = await _neo4jDataAccess.ExecuteWriteTransactionAsync(
            query, parameters);

        IRecord record = response.First();
        bool result = record[0].As<bool>();

        return result;
    }
}
