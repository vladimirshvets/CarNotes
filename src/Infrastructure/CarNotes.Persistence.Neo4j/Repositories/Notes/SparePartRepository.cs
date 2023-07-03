using AutoMapper;
using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;

namespace CarNotes.Persistence.Neo4j.Repositories.Notes;

public class SparePartRepository : INoteRepository<SparePart>
{
    private readonly IMapper _mapper;

    private readonly INeo4jDataAccess _neo4jDataAccess;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SparePartRepository"/> class.
    /// </summary>
    /// <param name="mapper">Mapper</param>
    /// <param name="neo4jDataAccess">Neo4j storage context</param>
    public SparePartRepository(
        IMapper mapper,
        INeo4jDataAccess neo4jDataAccess)
    {
        _mapper = mapper;
        _neo4jDataAccess = neo4jDataAccess;
    }

    /// <summary>
    /// Returns a list of spare part records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of spare part records.</returns>
    public async Task<IEnumerable<SparePart>> GetListAsync(Guid carId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage)<-[:MILE_MARKER { tag: $tag }]-(p:SparePart)
                RETURN p, m
                ORDER BY m.odometer DESC, m.date DESC";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "tag", "installation" }
        };

        var response = await _neo4jDataAccess.ExecuteReadDictionaryAsync(
                query, new List<string> { "p", "m" }, parameters);

        int sparePartsCount = response.Count / 2;
        List<SparePart> spareParts = new(sparePartsCount);
        for (int i = 0; i < sparePartsCount; i++)
        {
            SparePart sparePart = _mapper.Map<SparePart>(
                response[i * 2],
                opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[i * 2 + 1]));
            spareParts.Add(sparePart);
        }

        return spareParts;
    }

    /// <summary>
    /// Creates a new spare part record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="sparePart">Spare part data</param>
    /// <returns>A newly created instance of spare part.</returns>
    public async Task<SparePart> AddAsync(
        Guid carId, Guid mileageId, SparePart sparePart)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })
            CREATE
                (p:SparePart {
                    id: apoc.create.uuid(),
                    category: $category,
                    order_date: $orderDate,
                    purchase_date: $purchaseDate,
                    group: $group,
                    name: $name,
                    uom: $uom,
                    is_oe: $isOE,
                    oe_number: $oeNumber,
                    replacement_number: $replacementNumber,
                    manufacturer: $manufacturer,
                    country_of_origin: $countryOfOrigin,
                    price: $price,
                    qty: $qty,
                    total_amount: $totalAmount,
                    shop_website_url: $shopWebsiteUrl,
                    shop_address: $shopAddress,
                    production_date: $productionDate,
                    expiration_date: $expirationDate,
                    comment: $comment
                }),
                (c)-[:PART { created_at: timestamp() }]->(p),
                (p)-[:MILE_MARKER { created_at: timestamp(), tag: $tag }]->(m)
            RETURN p, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "tag", "installation" },
            { "mileageId", mileageId.ToString() },
            { "category", sparePart.Category },
            { "orderDate", sparePart.OrderDate },
            { "purchaseDate", sparePart.PurchaseDate },
            { "group", sparePart.Group },
            { "name", sparePart.Name },
            { "uom", sparePart.UoM },
            { "isOE", sparePart.IsOE },
            { "oeNumber", sparePart.OENumber },
            { "replacementNumber", sparePart.ReplacementNumber },
            { "manufacturer", sparePart.Manufacturer },
            { "countryOfOrigin", sparePart.CountryOfOrigin },
            { "price", sparePart.Price },
            { "qty", sparePart.Qty },
            { "totalAmount", sparePart.TotalAmount },
            { "shopWebsiteUrl", sparePart.ShopWebsiteUrl },
            { "shopAddress", sparePart.ShopAddress },
            { "productionDate", sparePart.ProductionDate },
            { "expirationDate", sparePart.ExpirationDate },
            { "comment", sparePart.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        SparePart newInstance = _mapper.Map<SparePart>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return newInstance;
    }

    /// <summary>
    /// Updates an existing spare part record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="sparePartId">Spare part identifier</param>
    /// <param name="sparePart">Spare part data</param>
    /// <returns>An updated instance of spare part.</returns>
    public async Task<SparePart> UpdateAsync(
        Guid carId, Guid mileageId, Guid sparePartId, SparePart sparePart)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(p:SparePart { id: $sparePartId })
            SET
                p.category = $category,
                p.order_date = $orderDate,
                p.purchase_date = $purchaseDate,
                p.group = $group,
                p.name = $name,
                p.uom = $uom,
                p.is_oe = $isOE,
                p.oe_number = $oeNumber,
                p.replacement_number = $replacementNumber,
                p.manufacturer = $manufacturer,
                p.country_of_origin = $countryOfOrigin,
                p.price = $price,
                p.qty = $qty,
                p.total_amount = $totalAmount,
                p.shop_website_url = $shopWebsiteUrl,
                p.shop_address = $shopAddress,
                p.production_date = $productionDate,
                p.expiration_date = $expirationDate,
                p.comment = $comment
            RETURN p, m";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "sparePartId", sparePartId.ToString() },
            { "category", sparePart.Category },
            { "orderDate", sparePart.OrderDate },
            { "purchaseDate", sparePart.PurchaseDate },
            { "group", sparePart.Group },
            { "name", sparePart.Name },
            { "uom", sparePart.UoM },
            { "isOE", sparePart.IsOE },
            { "oeNumber", sparePart.OENumber },
            { "replacementNumber", sparePart.ReplacementNumber },
            { "manufacturer", sparePart.Manufacturer },
            { "countryOfOrigin", sparePart.CountryOfOrigin },
            { "price", sparePart.Price },
            { "qty", sparePart.Qty },
            { "totalAmount", sparePart.TotalAmount },
            { "shopWebsiteUrl", sparePart.ShopWebsiteUrl },
            { "shopAddress", sparePart.ShopAddress },
            { "productionDate", sparePart.ProductionDate },
            { "expirationDate", sparePart.ExpirationDate },
            { "comment", sparePart.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        SparePart updatedInstance = _mapper.Map<SparePart>(
            response[0],
            opt => opt.Items["Mileage"] = _mapper.Map<Mileage>(response[1]));

        return updatedInstance;
    }

    /// <summary>
    /// Deletes an existing spare part record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <param name="sparePartId">Spare part identifier</param>
    /// <returns>true on success.</returns>
    public async Task<bool> DeleteAsync(
        Guid carId, Guid mileageId, Guid sparePartId)
    {
        string query =
            @"MATCH (c:Car { id: $carId })-[:MILE_MARKER]->(m:Mileage { id: $mileageId })<-[:MILE_MARKER]-(p:SparePart { id: $sparePartId })
            DETACH DELETE p
            RETURN true";

        var parameters = new Dictionary<string, object>
        {
            { "carId", carId.ToString() },
            { "mileageId", mileageId.ToString() },
            { "sparePartId", sparePartId.ToString() }
        };

        bool response =
            await _neo4jDataAccess.ExecuteWriteWithScalarResultAsync<bool>(
                query, parameters);

        return response;
    }
}
