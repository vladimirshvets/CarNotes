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
        CreateMap<INode, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(node => node.Properties["username"]))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(node => node.Properties["email"]))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(node => node.Properties["password_hash"]))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(node => node.Properties["firstname"]))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(node => node.Properties["lastname"]))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(
                node => ((ZonedDateTime)node.Properties["created_at"]).ToDateTimeOffset().DateTime))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(
                node => ((ZonedDateTime)node.Properties["updated_at"]).ToDateTimeOffset().DateTime));

        CreateMap<INode, Car>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(node => node.Properties["make"]))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(node => node.Properties["model"]))
            .ForMember(dest => dest.Generation, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("generation")
                        ? node.Properties["generation"]
                        : null))
            .ForMember(dest => dest.VIN, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("VIN")
                        ? node.Properties["VIN"]
                        : null))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("year")
                        ? node.Properties["year"]
                        : null))
            .ForMember(dest => dest.Plate, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("plate")
                        ? node.Properties["plate"]
                        : null))
            .ForMember(dest => dest.EngineType, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("engine_type")
                        ? Enum.Parse(typeof(EngineType), (string)node.Properties["engine_type"], true)
                        : null))
            .ForMember(dest => dest.OwnedFrom, opt => opt.ConvertUsing(
                new LocalDateValueConverter("owned_from"), node => node))
            .ForMember(dest => dest.OwnedTo, opt => opt.ConvertUsing(
                new LocalDateValueConverter("owned_to"), node => node))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("avatar_url")
                        ? node.Properties["avatar_url"]
                        : null));

        CreateMap<INode, Mileage>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(
                node => ((LocalDate)node.Properties["date"]).ToDateOnly()))
            .ForMember(dest => dest.OdometerValue, opt => opt.MapFrom(
                node => node.Properties["odometer"]));

        CreateMap<INode, LegalProcedure>()
            .ConstructUsing((src, ctx) => new LegalProcedure
                { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("group")
                        ? node.Properties["group"]
                        : null))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node.Properties["title"]))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(
                node => node.Properties["total_amount"]))
            .ForMember(dest => dest.ExpirationDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("expiration_date"), node => node))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("comment")
                        ? node.Properties["comment"]
                        : null));

        CreateMap<INode, Refueling>()
            .ConstructUsing((src, ctx) => new Refueling
                { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(node => node.Properties["volume"]))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(node => node.Properties["price"]))
            .ForMember(dest => dest.Distributor, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("distributor")
                        ? node.Properties["distributor"]
                        : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("address")
                        ? node.Properties["address"]
                        : null))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("comment")
                        ? node.Properties["comment"]
                        : null));

        CreateMap<INode, Service>()
            .ConstructUsing((src, ctx) => new Service
                { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node.Properties["title"]))
            .ForMember(dest => dest.StationName, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("station_name")
                        ? node.Properties["station_name"]
                        : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("address")
                        ? node.Properties["address"]
                        : null))
            .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("website_url")
                        ? node.Properties["website_url"]
                        : null))
            .ForMember(dest => dest.CostOfWork, opt => opt.MapFrom(
                node => node.Properties["cost_of_work"]))
            .ForMember(dest => dest.CostOfSpareParts, opt => opt.MapFrom(
                node => node.Properties["cost_of_spare_parts"]))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("comment")
                        ? node.Properties["comment"]
                        : null));

        CreateMap<INode, SparePart>()
            .ConstructUsing((src, ctx) => new SparePart
                { InstallationMileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(node => node.Properties["category"]))
            .ForMember(dest => dest.OrderDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("order_date"), node => node))
            .ForMember(dest => dest.PurchaseDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("purchase_date"), node => node))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("group")
                        ? node.Properties["group"]
                        : null))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(node => node.Properties["name"]))
            .ForMember(dest => dest.UoM, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("uom")
                        ? node.Properties["uom"]
                        : null))
            .ForMember(dest => dest.IsOE, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_oe")
                        ? node.Properties["is_oe"]
                        : null))
            .ForMember(dest => dest.OENumber, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("oe_number")
                        ? node.Properties["oe_number"]
                        : null))
            .ForMember(dest => dest.ReplacementNumber, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("replacement_number")
                        ? node.Properties["replacement_number"]
                        : null))
            .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("manufacturer")
                        ? node.Properties["manufacturer"]
                        : null))
            .ForMember(dest => dest.CountryOfOrigin, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("country_of_origin")
                        ? node.Properties["country_of_origin"]
                        : null))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(node => node.Properties["qty"]))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(node => node.Properties["price"]))
            .ForMember(dest => dest.ShopWebsiteUrl, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("shop_website_url")
                        ? node.Properties["shop_website_url"]
                        : null))
            .ForMember(dest => dest.ShopAddress, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("shop_address")
                        ? node.Properties["shop_address"]
                        : null))
            .ForMember(dest => dest.ProductionDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("production_date"), node => node))
            .ForMember(dest => dest.ExpirationDate, opt => opt.ConvertUsing(
                new LocalDateValueConverter("expiration_date"), node => node))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("comment")
                        ? node.Properties["comment"]
                        : null));

        CreateMap<INode, TextNote>()
            .ConstructUsing((src, ctx) => new TextNote
                { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(node => node.Properties["title"]))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(node => node.Properties["tag"]))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(node => node.Properties["text"]));

        CreateMap<INode, Washing>()
            .ConstructUsing((src, ctx) => new Washing
                { Mileage = ctx.Items["Mileage"] as Mileage ?? null })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(node => node.Properties["id"]))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("title")
                        ? node.Properties["title"]
                        : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("address")
                        ? node.Properties["address"]
                        : null))
            .ForMember(dest => dest.IsContact, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_contact")
                        ? node.Properties["is_contact"]
                        : null))
            .ForMember(dest => dest.IsDegreaserUsed, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_degreaser_used")
                        ? node.Properties["is_degreaser_used"]
                        : null))
            .ForMember(dest => dest.IsPolishUsed, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_polish_used")
                        ? node.Properties["is_polish_used"]
                        : null))
            .ForMember(dest => dest.IsAntiRainUsed, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_antirain_used")
                        ? node.Properties["is_antirain_used"]
                        : null))
            .ForMember(dest => dest.IsInteriorCleaned, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("is_interior_cleaned")
                        ? node.Properties["is_interior_cleaned"]
                        : null))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(
                node => node.Properties["total_amount"]))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                node => node.Properties.ContainsKey("comment")
                        ? node.Properties["comment"]
                        : null));
    }
}
