using CarNotes.Domain.Interfaces;
using CarNotes.Persistence.Neo4j.Repositories;
using FluentAssertions;
using Moq;

namespace CarNotes.Persistence.Neo4j.UnitTests.Systems.Repositories;

public class StatsRepositoryTests
{
    [Fact]
    public async Task GetNumberOfRecords_On_PassNoTypes_ShouldReturn_Zero_Async()
    {
        // Arrange.
        Guid carId = Guid.NewGuid();
        IEnumerable<string> types = Array.Empty<string>();

        var mockNeo4jDataAccess = new Mock<INeo4jDataAccess>();
        var sut = new StatsRepository(mockNeo4jDataAccess.Object);

        // Act.
        int result = await sut.GetNumberOfRecordsAsync(carId, types);

        // Assert.
        result.Should().Be(0);
    }
}
