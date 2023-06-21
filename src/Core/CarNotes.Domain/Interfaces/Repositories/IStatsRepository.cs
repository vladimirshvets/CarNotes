namespace CarNotes.Domain.Interfaces.Repositories
{
    public interface IStatsRepository
    {
        /// <summary>
        /// Get number of records of a specified relation types.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="relationTypes">Collection of relation types</param>
        /// <returns>Number of records found.</returns>
        Task<int> GetNumberOfRecordsAsync(
            Guid carId, IEnumerable<string> relationTypes);

        /// <summary>
        /// Get total volume of consumed fuel.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <returns>Total fuel consumed.</returns>
        Task<double> GetTotalFuelConsumed(Guid carId);
    }
}
