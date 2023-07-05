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

    public async Task<int> TotalNumberOfUsersAsync()
        => await _userRepository.GetNumberOfUsersAsync();

    public async Task<int> TotalNumberOfCarsAsync()
        => await _carRepository.GetNumberOfCarsAsync();

    public async Task<int> NumberOfActionRecordsAsync(Guid carId)
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

    public async Task<double> AverageFuelConsumptionAsync(Guid carId)
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
            await _statsRepository.GetTotalFuelConsumedAsync(carId);

        return totalFuelConsumed * 100 / distance;
    }

    public async Task<int> TotalDistanceAsync(Guid carId)
        => await _mileageRepository.GetDeltaOdometerValueAsync(carId);

    public async Task<int> DistancePerMonthAsync(Guid carId)
    {
        double monthsOfOwnership = await MonthsOfOwnershipAsync(carId);

        switch (monthsOfOwnership)
        {
            case 0:
                return 0;

            case < 1:
                return await TotalDistanceAsync(carId);

            default:
                int totalDistance = await TotalDistanceAsync(carId);
                return (int)(totalDistance / monthsOfOwnership);
        }
    }

    public async Task<double> MoneySpentInTotalAsync(Guid carId)
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

        return await _statsRepository.GetTotalMoneySpentAsync(carId, noteTypes);
    }

    public async Task<double> MoneySpentByNoteTypeAsync(
        Guid carId, string noteType)
    {
        string[] noteTypes = new[] { noteType };

        return await _statsRepository.GetTotalMoneySpentAsync(carId, noteTypes);
    }

    public async Task<double> MoneySpentPerKmAsync(Guid carId)
    {
        int totalDistance = await TotalDistanceAsync(carId);
        if (totalDistance == 0)
        {
            return 0;
        }

        double moneyTotal = await MoneySpentInTotalAsync(carId);

        return moneyTotal / totalDistance;
    }

    public async Task<double> MoneySpentPerMonthAsync(Guid carId)
    {
        double monthsOfOwnership = await MonthsOfOwnershipAsync(carId);

        switch (monthsOfOwnership)
        {
            case 0:
                return 0;

            case < 1:
                return await MoneySpentInTotalAsync(carId);

            default:
                double moneyTotal = await MoneySpentInTotalAsync(carId);
                return moneyTotal / monthsOfOwnership;
        }
    }

    private async Task<double> MonthsOfOwnershipAsync(Guid carId)
    {
        Car car = await _carRepository.GetAsync(carId)
            ?? throw new ModelNotFoundException();

        if (car.OwnedFrom == null)
        {
            return 0;
        }

        DateOnly ownedFrom = (DateOnly)car.OwnedFrom;
        DateOnly ownedTo = car.OwnedTo ?? DateOnly.FromDateTime(DateTime.UtcNow);
        int daysOfOwnerShip = ownedTo.DayNumber - ownedFrom.DayNumber;
        if (daysOfOwnerShip == 0)
        {
            return 0;
        }

        return daysOfOwnerShip / AVG_DAYS_IN_MONTH;
    }

    public async Task<IEnumerable<Mileage>> GetLatestMileagesAsync(
        Guid carId, int skip, int take)
    {
        string[] noteTypes = new[]
        {
            nameof(LegalProcedure),
            nameof(Refueling),
            nameof(TextNote),
            nameof(Service),
            nameof(SparePart),
            nameof(Washing)
        };

        return await _statsRepository.GetMileagesWithRelatedNotesAsync(
            carId, noteTypes, skip, take);
    }
}
