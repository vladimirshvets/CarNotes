using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Data.Api;

public interface IMileageRepository
{
    /// <summary>
    /// Returns a list of mileage records of a specified car.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>Collection of mileage records.</returns>
    Task<IEnumerable<Mileage>> GetListAsync(Guid carId);

    /// <summary>
    /// Creates a new mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileage">Mileage data</param>
    /// <returns>A newly created instance of mileage.</returns>
    Task<Mileage> AddAsync(Guid carId, Mileage mileage);

    /// <summary>
    /// Updates an existing mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageDate">Current mileage date</param>
    /// <param name="mileageValue">Current mileage value</param>
    /// <param name="mileage">Updated mileage data</param>
    /// <returns>Updated instance of mileage.</returns>
    Task<Mileage> UpdateAsync(
        Guid carId, DateOnly mileageDate, long mileageValue, Mileage mileage);

    /// <summary>
    /// Deletes an existing mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <returns>true on success.</returns>
    Task<bool> DeleteAsync(Guid carId, Guid mileageId);

    /// <summary>
    /// Returns the number of records related to specified mileage record.
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <param name="mileageId">Mileage identifier</param>
    /// <returns>Count of related records.</returns>
    Task<int> GetRelatedRecordsCountAsync(Guid carId, Guid mileageId);
}
