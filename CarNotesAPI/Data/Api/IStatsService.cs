namespace CarNotesAPI.Data.Api
{
    public interface IStatsService
    {
        Task<double> AverageFuelConsumption(Guid carId);

        Task<int> NumberOfActionRecords(Guid carId);
    }
}
