namespace CarNotes.Domain.Interfaces.Services
{
    public interface IStatsService
    {
        Task<int> TotalNumberOfCars();

        Task<int> TotalNumberOfUsers();

        Task<int> NumberOfActionRecords(Guid carId);

        Task<double> AverageFuelConsumption(Guid carId);

        Task<int> OdometerDelta(Guid carId);

        Task<double> MoneySpentInTotal(Guid carId);

        Task<double> MoneySpentByNoteType(Guid carId, string noteType);

        Task<double> MoneySpentPerKm(Guid carId);

        Task<double> MoneySpentPerMonth(Guid carId);
    }
}
