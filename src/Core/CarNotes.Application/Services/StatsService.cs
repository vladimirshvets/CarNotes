using CarNotes.Domain.Common.Exceptions;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;

namespace CarNotes.Application.Services;

public class StatsService : IStatsService
{
    /// <summary>
    /// The average number of days in a month, calculated as 365.25 / 12.
    /// </summary>
    const double AVG_DAYS_IN_MONTH = 30.4375;

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
        => await _userRepository.GetNumberOfUsersAsync();

    public async Task<int> TotalNumberOfCars()
        => await _carRepository.GetNumberOfCarsAsync();

    public async Task<int> NumberOfActionRecords(Guid carId)
    {
        string[] relationTypes = new[]
        {
            "FUEL",
            "WASH",
            "SERVICE",
            "PART",
            "TEXT",
            "LEGAL"
        };

        return await _statsRepository.GetNumberOfRecordsAsync(
            carId, relationTypes);
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

    public async Task<int> OdometerDelta(Guid carId)
        => await _mileageRepository.GetDeltaOdometerValueAsync(carId);

    public async Task<double> MoneySpentInTotal(Guid carId)
    {
        // A collection of note types that contain the TotalAmount property.
        string[] noteTypes = new[]
        {
            nameof(LegalProcedure),
            nameof(Refueling),
            nameof(Service),
            nameof(SparePart),
            nameof(Washing)
        };

        return await _statsRepository.GetTotalMoneySpent(carId, noteTypes);
    }

    public async Task<double> MoneySpentByNoteType(Guid carId, string noteType)
    {
        string[] noteTypes = new[] { noteType };

        return await _statsRepository.GetTotalMoneySpent(carId, noteTypes);
    }

    public async Task<double> MoneySpentPerKm(Guid carId)
    {
        int odometerDelta = await OdometerDelta(carId);
        if (odometerDelta == 0)
        {
            return 0;
        }

        double moneyTotal = await MoneySpentInTotal(carId);

        return moneyTotal / odometerDelta;
    }

    public async Task<double> MoneySpentPerMonth(Guid carId)
    {
        Car car = await _carRepository.GetAsync(carId)
            ?? throw new ModelNotFoundException();

        if (car.OwnedFrom == null)
        {
            return 0;
        }

        DateOnly ownedFrom = (DateOnly)car.OwnedFrom;
        DateOnly ownedTo = car.OwnedTo ?? DateOnly.FromDateTime(DateTime.UtcNow);
        int ownershipPeriod = ownedTo.DayNumber - ownedFrom.DayNumber;
        if (ownershipPeriod == 0)
        {
            return 0;
        }

        double moneyTotal = await MoneySpentInTotal(carId);

        return moneyTotal / ownershipPeriod * AVG_DAYS_IN_MONTH;
    }
}
