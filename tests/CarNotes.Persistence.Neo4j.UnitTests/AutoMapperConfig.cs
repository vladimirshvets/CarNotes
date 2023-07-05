using AutoMapper;

namespace CarNotes.Persistence.Neo4j.UnitTests
{
    public class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DataAccessMappingProfile>();
            });

            return configuration.CreateMapper();
        }
    }
}
