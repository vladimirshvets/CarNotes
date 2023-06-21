using CarNotes.Domain.Models.Notes;

namespace CarNotes.Application.UnitTests.Fixtures
{
    public static class RefuelingFixtures
    {
        public static List<Refueling> GetThreeRefuelingsWithATotalVolumeOf65()
        {
            return new List<Refueling>
            {
                new Refueling
                {
                    Volume = 20,
                    Price = 2.0
                },
                new Refueling
                {
                    Volume = 30,
                    Price = 2.0
                },
                new Refueling
                {
                    Volume = 15,
                    Price = 2.0
                }
            };
        }
    }
}
