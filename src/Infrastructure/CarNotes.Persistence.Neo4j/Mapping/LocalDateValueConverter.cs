using AutoMapper;
using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j.Mapping
{
    public class LocalDateValueConverter : IValueConverter<Dictionary<string, object>, DateOnly?>
    {
        private readonly string? _key;

        public LocalDateValueConverter()
        {
        }

        public LocalDateValueConverter(string key)
        {
            _key = key;
        }

        public DateOnly? Convert(
            Dictionary<string, object> sourceMember,
            ResolutionContext context)
        {
            if (_key != null && sourceMember.TryGetValue(_key, out object? value))
            {
                return ((LocalDate)value).ToDateOnly();
            }

            return null;
        }
    }
}
