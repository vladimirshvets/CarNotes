namespace CarNotes.Domain.Interfaces.Services
{
    public interface IStatsService
    {
        Task<int> TotalNumberOfCars();

        Task<int> TotalNumberOfUsers();

        Task<int> NumberOfActionRecords(Guid carId);

        Task<double> AverageFuelConsumption(Guid carId);

        Task<int> OdometerDelta(Guid carId);
    }
}
