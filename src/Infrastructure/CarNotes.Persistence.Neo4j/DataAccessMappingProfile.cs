using AutoMapper;
using CarNotes.Domain.Enums;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using CarNotes.Persistence.Neo4j.Mapping;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j;

public class DataAccessMappingProfile : Profile
{
    public DataAccessMappingProfile()
    {
        CreateMap<Dictionary<string, object>, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(node => node["username"]))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(node => node["email"]))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(node => node["password_hash"]))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(node => node["firstname"]))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(node => node["lastname"]))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(
                node => ((ZonedDateTime)node["created_at"]).ToDateTimeOffset().DateTime))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(
                node => ((ZonedDateTime)node["updated_at"]).ToDateTimeOffset().DateTime));

        CreateMap<Dictionary<string, object>, Car>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(node => node["make"]))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(node => node["model"]))
            .ForMember(dest => dest.Generation, opt => opt.MapFrom(
                node => node.ContainsKey("generation") ? node["generation"] : null))
            .ForMember(dest => dest.VIN, opt => opt.MapFrom(
                node => node.ContainsKey("VIN") ? node["VIN"] : null))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(
                node =>  node.ContainsKey("year") ? node["year"] : null))
            .ForMember(dest => dest.Plate, opt => opt.MapFrom(
                node => node.ContainsKey("plate") ? node["plate"] : null))
            .ForMember(dest => dest.EngineType, opt => opt.MapFrom(node =>
                node.ContainsKey("engine_type") ? Enum.Parse(typeof(EngineType), (string)node["engine_type"], true): null))
            .ForMember(dest => dest.OwnedFrom, opt => opt.ConvertUsing(
                new LocalDateValueConverter("owned_from"), node => node))
            .ForMember(dest => dest.OwnedTo, opt => opt.ConvertUsing(
                new LocalDateValueConverter("owned_to"), node => node))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(
                node => node.ContainsKey("avatar_url") ? node["avatar_url"] : null));

        CreateMap<Dictionary<string, object>, Mileage>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(node => ((LocalDate)node["date"]).ToDateOnly()))
            .ForMember(dest => dest.OdometerValue, opt => opt.MapFrom(node => node["odometer"]));

        CreateMap<Dictionary<string, object>, LegalProcedure>()
            .ConstructUsing((src, ctx) => new LegalProcedure { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(
                node => node.ContainsKey("group") ? node["group"] : null))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node["title"]))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(node => node["total_amount"]))
            .ForMember(dest => dest.ExpirationDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("expiration_date"), node => node))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.ContainsKey("comment") ? node["comment"] : null));

        CreateMap<Dictionary<string, object>, Refueling>()
            .ConstructUsing((src, ctx) => new Refueling { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(node => node["volume"]))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(node => node["price"]))
            .ForMember(dest => dest.Distributor, opt => opt.MapFrom(
                node => node.ContainsKey("distributor") ? node["distributor"] : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.ContainsKey("address") ? node["address"] : null))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.ContainsKey("comment") ? node["comment"] : null));

        CreateMap<Dictionary<string, object>, Service>()
            .ConstructUsing((src, ctx) => new Service { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node["title"]))
            .ForMember(dest => dest.StationName, opt => opt.MapFrom(
                node => node.ContainsKey("station_name") ? node["station_name"] : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.ContainsKey("address") ? node["address"] : null))
            .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(
                node => node.ContainsKey("website_url") ? node["website_url"] : null))
            .ForMember(dest => dest.CostOfWork, opt => opt.MapFrom(node => node["cost_of_work"]))
            .ForMember(dest => dest.CostOfSpareParts, opt => opt.MapFrom(node => node["cost_of_spare_parts"]))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.ContainsKey("comment") ? node["comment"] : null));

        CreateMap<Dictionary<string, object>, SparePart>()
            .ConstructUsing((src, ctx) => new SparePart { InstallationMileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(node => node["category"]))
            .ForMember(dest => dest.OrderDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("order_date"), node => node))
            .ForMember(dest => dest.PurchaseDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("purchase_date"), node => node))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(
                node => node.ContainsKey("group") ? node["group"] : null))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(node => node["name"]))
            .ForMember(dest => dest.UoM, opt => opt.MapFrom(
                node => node.ContainsKey("uom") ? node["uom"] : null))
            .ForMember(dest => dest.IsOE, opt => opt.MapFrom(
                node => node.ContainsKey("is_oe") ? node["is_oe"] : null))
            .ForMember(dest => dest.OENumber, opt => opt.MapFrom(
                node => node.ContainsKey("oe_number") ? node["oe_number"] : null))
            .ForMember(dest => dest.ReplacementNumber, opt => opt.MapFrom(
                node => node.ContainsKey("replacement_number") ? node["replacement_number"] : null))
            .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(
                node => node.ContainsKey("manufacturer") ? node["manufacturer"] : null))
            .ForMember(dest => dest.CountryOfOrigin, opt => opt.MapFrom(
                node => node.ContainsKey("country_of_origin") ? node["country_of_origin"] : null))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(node => node["qty"]))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(node => node["price"]))
            .ForMember(dest => dest.ShopWebsiteUrl, opt => opt.MapFrom(
                node => node.ContainsKey("shop_website_url") ? node["shop_website_url"] : null))
            .ForMember(dest => dest.ShopAddress, opt => opt.MapFrom(
                node => node.ContainsKey("shop_address") ? node["shop_address"] : null))
            .ForMember(dest => dest.ProductionDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("production_date"), node => node))
            .ForMember(dest => dest.ExpirationDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("expiration_date"), node => node))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.ContainsKey("comment") ? node["comment"] : null));

        CreateMap<Dictionary<string, object>, TextNote>()
            //.ConstructUsing((src, ctx) => new TextNote { Mileage = ctx.TryGetItems(out var items) ? items.ContainsKey("Mileage") ? items["Mileage"] as Mileage : null : null })
            .ConstructUsing((src, ctx) => new TextNote { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node["title"]))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(node => node["tag"]))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(node => node["text"]));

        CreateMap<Dictionary<string, object>, Washing>()
            .ConstructUsing((src, ctx) => new Washing { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(
                node => node.ContainsKey("title") ? node["title"] : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.ContainsKey("address") ? node["address"] : null))
            .ForMember(dest => dest.IsContact, opt => opt.MapFrom(
                node => node.ContainsKey("is_contact") ? node["is_contact"] : null))
            .ForMember(dest => dest.IsDegreaserUsed, opt => opt.MapFrom(
                node => node.ContainsKey("is_degreaser_used") ? node["is_degreaser_used"] : null))
            .ForMember(dest => dest.IsPolishUsed, opt => opt.MapFrom(
                node => node.ContainsKey("is_polish_used") ? node["is_polish_used"] : null))
            .ForMember(dest => dest.IsAntiRainUsed, opt => opt.MapFrom(
                node => node.ContainsKey("is_antirain_used") ? node["is_antirain_used"] : null))
            .ForMember(dest => dest.IsInteriorCleaned, opt => opt.MapFrom(
                node => node.ContainsKey("is_interior_cleaned") ? node["is_interior_cleaned"] : null))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(node => node["total_amount"]))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.ContainsKey("comment") ? node["comment"] : null));
    }
}
