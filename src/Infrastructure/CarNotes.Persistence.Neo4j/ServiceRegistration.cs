using CarNotes.Domain.Interfaces;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.Persistence.Neo4j.Mapping;
using CarNotes.Persistence.Neo4j.Repositories;
using CarNotes.Persistence.Neo4j.Repositories.Notes;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddNeo4jPersistence(
            this IServiceCollection services,
            Action<Neo4jOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var neo4jOptions = new Neo4jOptions();
            configureOptions.Invoke(neo4jOptions);
            services.AddSingleton(neo4jOptions);
            services.AddSingleton(GraphDatabase.Driver(
                neo4jOptions.Connection,
                AuthTokens.Basic(neo4jOptions.User, neo4jOptions.Password)));

            services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IMileageRepository, MileageRepository>();
            services.AddTransient<IStatsRepository, StatsRepository>();
            services.AddTransient<INoteRepository<LegalProcedure>, LegalProcedureRepository>();
            services.AddTransient<INoteRepository<Refueling>, RefuelingRepository>();
            services.AddTransient<INoteRepository<Service>, ServiceRepository>();
            services.AddTransient<INoteRepository<SparePart>, SparePartRepository>();
            services.AddTransient<INoteRepository<TextNote>, TextNoteRepository>();
            services.AddTransient<INoteRepository<Washing>, WashingRepository>();

            services.AddTransient<LocalDateValueConverter>();
            services.AddAutoMapper(typeof(DataAccessMappingProfile));

            return services;
        }
    }
}
