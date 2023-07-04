using CarNotes.Application.Services;
using CarNotes.Domain.Interfaces.Repositories;
using Moq;

namespace CarNotes.Application.UnitTests.Systems.Services;

public class StatsServiceTests
{
    [Fact]
    public async Task AverageFuelConsumption_On_ZeroDistance_ShouldBe_Zero()
    {
        // Arrange.
        Guid carId = Guid.NewGuid();
        int minOdometerValue = 0;
        int maxOdometerValue = 0;
        int refuelingSum = 65;
        double expectedResult = 0;

        var mockCarRepository = new Mock<ICarRepository>();
        var mockMileageRepository = new Mock<IMileageRepository>();
        var mockStatsRepository = new Mock<IStatsRepository>();
        var mockUserRepository = new Mock<IUserRepository>();

        mockMileageRepository
            .Setup(repo => repo.GetMinOdometerValueAsync(carId))
            .ReturnsAsync(minOdometerValue);
        mockMileageRepository
            .Setup(repo => repo.GetMaxOdometerValueAsync(carId))
            .ReturnsAsync(maxOdometerValue);
        mockStatsRepository
            .Setup(repo => repo.GetTotalFuelConsumedAsync(carId))
            .ReturnsAsync(refuelingSum);

        var sut = new StatsService(
            mockCarRepository.Object,
            mockMileageRepository.Object,
            mockStatsRepository.Object,
            mockUserRepository.Object);

        // Act.
        double result = await sut.AverageFuelConsumptionAsync(carId);

        // Assert.
        Assert.Equal(expectedResult, result);
        mockMileageRepository.Verify(
            repo => repo.GetMinOdometerValueAsync(carId), Times.Once);
        mockMileageRepository.Verify(
            repo => repo.GetMaxOdometerValueAsync(carId), Times.Once);
        mockStatsRepository.Verify(
            repo => repo.GetTotalFuelConsumedAsync(carId), Times.Never);
    }

    [Fact]
    public async Task AverageFuelConsumption_On_DistanceOf1000km_And_RefuelingsWithATotalVolumeOf65_ShouldBe_6point5()
    {
        // Arrange.
        Guid carId = Guid.NewGuid();
        int minOdometerValue = 101000;
        int maxOdometerValue = 102000;
        int refuelingSum = 65;
        double expectedResult = 6.5;

        var mockCarRepository = new Mock<ICarRepository>();
        var mockMileageRepository = new Mock<IMileageRepository>();
        var mockStatsRepository = new Mock<IStatsRepository>();
        var mockUserRepository = new Mock<IUserRepository>();

        mockMileageRepository
            .Setup(repo => repo.GetMinOdometerValueAsync(carId))
            .ReturnsAsync(minOdometerValue);
        mockMileageRepository
            .Setup(repo => repo.GetMaxOdometerValueAsync(carId))
            .ReturnsAsync(maxOdometerValue);
        mockStatsRepository
            .Setup(repo => repo.GetTotalFuelConsumedAsync(carId))
            .ReturnsAsync(refuelingSum);

        var sut = new StatsService(
            mockCarRepository.Object,
            mockMileageRepository.Object,
            mockStatsRepository.Object,
            mockUserRepository.Object);

        // Act.
        double result = await sut.AverageFuelConsumptionAsync(carId);

        // Assert.
        Assert.Equal(expectedResult, result);
        mockMileageRepository.Verify(
            repo => repo.GetMinOdometerValueAsync(carId), Times.Once);
        mockMileageRepository.Verify(
            repo => repo.GetMaxOdometerValueAsync(carId), Times.Once);
        mockStatsRepository.Verify(
            repo => repo.GetTotalFuelConsumedAsync(carId), Times.Once);
    }
}
