using CarNotes.Domain.Models;

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
        Task<double> GetTotalFuelConsumedAsync(Guid carId);

        /// <summary>
        /// Get total amount of money spent.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="noteTypes">Collection of note types</param>
        /// <returns>Total amount spent.</returns>
        Task<double> GetTotalMoneySpentAsync(
            Guid carId, IEnumerable<string> noteTypes);

        /// <summary>
        /// Get latest mileages together with related notes.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="noteTypes">Collection of note types</param>
        /// <param name="skip">Number of mileage records to skip</param>
        /// <param name="take">Number of mileage records to take</param>
        /// <returns>Collection of mileages with linked notes.</returns>
        Task<IEnumerable<Mileage>> GetMileagesWithRelatedNotesAsync(
            Guid carId,
            IEnumerable<string> noteTypes,
            int skip,
            int take);
    }
}
