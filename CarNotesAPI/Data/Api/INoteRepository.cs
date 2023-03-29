using CarNotesAPI.Data.Models.Notes;

namespace CarNotesAPI.Data.Api
{
    public interface INoteRepository<T> where T : Note
    {
        /// <summary>
        /// Returns a list of note records of a specified car.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <returns>Collection of note records.</returns>
        Task<IEnumerable<T>> GetListAsync(Guid carId);

        /// <summary>
        /// Creates a new note record.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="mileageId">Mileage identifier</param>
        /// <param name="note">Note data</param>
        /// <returns>A newly created instance of note.</returns>
        Task<T> AddAsync(Guid carId, Guid mileageId, T note);

        /// <summary>
        /// Updates an existing refueling record.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="mileageId">Mileage identifier</param>
        /// <param name="noteId">Note identifier</param>
        /// <param name="note">Note data</param>
        /// <returns>An updated instance of note.</returns>
        Task<T> UpdateAsync(Guid carId, Guid mileageId, Guid noteId, T note);

        /// <summary>
        /// Deletes an existing note record.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="mileageId">Mileage identifier</param>
        /// <param name="noteId">Note identifier</param>
        /// <returns>true on success.</returns>
        Task<bool> DeleteAsync(Guid carId, Guid mileageId, Guid noteId);
    }
}

