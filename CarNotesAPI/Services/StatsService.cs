using CarNotesAPI.Data.Api;

namespace CarNotesAPI.Services;

public class StatsService : IStatsService
{
    private readonly IMileageRepository _mileageRepository;

    private readonly IStatsRepository _statsRepository;

    public StatsService(
        IMileageRepository mileageRepository,
        IStatsRepository statsRepository)
    {
        _mileageRepository = mileageRepository;
        _statsRepository = statsRepository;
    }

    public async Task<double> AverageFuelConsumption(Guid carId)
    {
        int minOdometerValue =
            await _mileageRepository.GetMinOdometerValueAsync(carId);
        int maxOdometerValue =
            await _mileageRepository.GetMaxOdometerValueAsync(carId);
        int distance = maxOdometerValue - minOdometerValue;
        if (distance == 0)
        {
            return 0;
        }

        double totalFuelConsumed =
            await _statsRepository.GetTotalFuelConsumed(carId);

        return totalFuelConsumed * 100 / distance;
    }

    public async Task<int> NumberOfActionRecords(Guid carId)
    {
        string[] relationTypes = new[]
        {
            "FUEL",
            "WASH",
            "SERVICE",
            "LEGAL"
        };

        return await _statsRepository.GetNumberOfRecordsAsync(
            carId, relationTypes);
    }
}
