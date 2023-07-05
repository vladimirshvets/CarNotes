using CarNotes.Domain.Models;

namespace CarNotes.Domain.Interfaces.Services
{
    public interface IStatsService
    {
        Task<int> TotalNumberOfCarsAsync();

        Task<int> TotalNumberOfUsersAsync();

        Task<int> NumberOfActionRecordsAsync(Guid carId);

        Task<double> AverageFuelConsumptionAsync(Guid carId);

        Task<int> TotalDistanceAsync(Guid carId);

        Task<int> DistancePerMonthAsync(Guid carId);

        Task<double> MoneySpentInTotalAsync(Guid carId);

        Task<double> MoneySpentByNoteTypeAsync(Guid carId, string noteType);

        Task<double> MoneySpentPerKmAsync(Guid carId);

        Task<double> MoneySpentPerMonthAsync(Guid carId);

        Task<IEnumerable<Mileage>> GetLatestMileagesAsync(
            Guid carId, int skip, int take);
    }
}
