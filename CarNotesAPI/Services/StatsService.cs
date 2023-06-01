using CarNotesAPI.Data.Api;

namespace CarNotesAPI.Services;

public class StatsService : IStatsService
{
    private readonly ICarRepository _carRepository;

    private readonly IMileageRepository _mileageRepository;

    private readonly IStatsRepository _statsRepository;

    private readonly IUserRepository _userRepository;

    public StatsService(
        ICarRepository carRepository,
        IMileageRepository mileageRepository,
        IStatsRepository statsRepository,
        IUserRepository userRepository)
    {
        _carRepository = carRepository;
        _mileageRepository = mileageRepository;
        _statsRepository = statsRepository;
        _userRepository = userRepository;
    }

    public async Task<int> TotalNumberOfUsers()
    {
        return await _userRepository.GetNumberOfUsersAsync();
    }

    public async Task<int> TotalNumberOfCars()
    {
        return await _carRepository.GetNumberOfCarsAsync();
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
