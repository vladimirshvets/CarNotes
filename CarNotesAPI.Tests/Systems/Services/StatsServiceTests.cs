using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Services;
using Moq;

namespace CarNotesAPI.Tests.Systems.Services;

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

        var mockMileageRepository = new Mock<IMileageRepository>();
        var mockStatsRepository = new Mock<IStatsRepository>();

        mockMileageRepository
            .Setup(repo => repo.GetMinOdometerValueAsync(carId))
            .ReturnsAsync(minOdometerValue);
        mockMileageRepository
            .Setup(repo => repo.GetMaxOdometerValueAsync(carId))
            .ReturnsAsync(maxOdometerValue);
        mockStatsRepository
            .Setup(repo => repo.GetTotalFuelConsumed(carId))
            .ReturnsAsync(refuelingSum);

        var sut = new StatsService(
            mockMileageRepository.Object,
            mockStatsRepository.Object);

        // Act.
        double result = await sut.AverageFuelConsumption(carId);

        // Assert.
        Assert.Equal(expectedResult, result);
        mockMileageRepository.Verify(
            repo => repo.GetMinOdometerValueAsync(carId), Times.Once);
        mockMileageRepository.Verify(
            repo => repo.GetMaxOdometerValueAsync(carId), Times.Once);
        mockStatsRepository.Verify(
            repo => repo.GetTotalFuelConsumed(carId), Times.Never);
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

        var mockMileageRepository = new Mock<IMileageRepository>();
        var mockStatsRepository = new Mock<IStatsRepository>();

        mockMileageRepository
            .Setup(repo => repo.GetMinOdometerValueAsync(carId))
            .ReturnsAsync(minOdometerValue);
        mockMileageRepository
            .Setup(repo => repo.GetMaxOdometerValueAsync(carId))
            .ReturnsAsync(maxOdometerValue);
        mockStatsRepository
            .Setup(repo => repo.GetTotalFuelConsumed(carId))
            .ReturnsAsync(refuelingSum);

        var sut = new StatsService(
            mockMileageRepository.Object,
            mockStatsRepository.Object);

        // Act.
        double result = await sut.AverageFuelConsumption(carId);

        // Assert.
        Assert.Equal(expectedResult, result);
        mockMileageRepository.Verify(
            repo => repo.GetMinOdometerValueAsync(carId), Times.Once);
        mockMileageRepository.Verify(
            repo => repo.GetMaxOdometerValueAsync(carId), Times.Once);
        mockStatsRepository.Verify(
            repo => repo.GetTotalFuelConsumed(carId), Times.Once);
    }
}
