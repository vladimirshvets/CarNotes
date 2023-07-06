using AutoMapper;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Mapping
{
    public class LocalDateValueConverter : IValueConverter<INode, DateOnly?>
    {
        private readonly string? _key;

        public LocalDateValueConverter()
        {
        }

        public LocalDateValueConverter(string key)
        {
            _key = key;
        }

        public DateOnly? Convert(INode sourceMember, ResolutionContext context)
        {
            if (_key != null &&
                sourceMember.Properties.TryGetValue(_key, out object? value))
            {
                return ((LocalDate)value).ToDateOnly();
            }

            return null;
        }
    }
}
