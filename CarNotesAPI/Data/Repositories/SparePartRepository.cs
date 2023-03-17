using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Repositories;

public class SparePartRepository
{
    private readonly INeo4jDataAccess _neo4jDataAccess;

    private readonly ILogger<SparePartRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SparePartRepository"/> class.
    /// </summary>
    public SparePartRepository(
        INeo4jDataAccess neo4jDataAccess,
        ILogger<SparePartRepository> logger)
    {
        _neo4jDataAccess = neo4jDataAccess;
        _logger = logger;
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
            SparePart sparePart = new(response[i * 2])
            {
                Mileage = new Mileage(response[i * 2 + 1])
            };
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
            { "shopWebsiteUrl", sparePart.ShopWebsiteUrl },
            { "shopAddress", sparePart.ShopAddress },
            { "productionDate", sparePart.ProductionDate },
            { "expirationDate", sparePart.ExpirationDate },
            { "comment", sparePart.Comment }
        };

        var response = await _neo4jDataAccess.ExecuteWriteWithListResultAsync(
            query, parameters);

        SparePart newInstance = new(response[0])
        {
            InstallationMileage = new Mileage(response[1])
        };

        return newInstance;
    }
}
