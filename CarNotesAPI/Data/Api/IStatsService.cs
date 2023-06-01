namespace CarNotesAPI.Data.Api
{
    public interface IStatsService
    {
        Task<int> TotalNumberOfCars();

        Task<int> TotalNumberOfUsers();

        Task<double> AverageFuelConsumption(Guid carId);

        Task<int> NumberOfActionRecords(Guid carId);
    }
}
