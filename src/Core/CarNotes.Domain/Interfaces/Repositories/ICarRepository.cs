using CarNotes.Domain.Models;

namespace CarNotes.Domain.Interfaces.Repositories
{
    public interface ICarRepository
    {
        /// <summary>
        /// Returns a list of cars of a specified user.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Collection of cars.</returns>
        Task<IEnumerable<Car>> GetListAsync(Guid userId);

        /// <summary>
        /// Returns a car record by car ID of a specified user.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="carId">Car identifier</param>
        /// <returns>An instance of car.</returns>
        Task<Car?> GetAsync(Guid userId, Guid carId);

        /// <summary>
        /// Creates a new car record.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="car">Car data</param>
        /// <returns>A newly created instance of car.</returns>
        Task<Car> AddAsync(Guid userId, Car car);

        /// <summary>
        /// Updates an existing car record.
        /// </summary>
        /// <param name="carId">Car identifier</param>
        /// <param name="car">Updated car data</param>
        /// <returns>Updated instance of car.</returns>
        Task<Car> UpdateAsync(Guid carId, Car car);

        /// <summary>
        /// Deletes a car with all relations and related notes.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="carId">Car identifier</param>
        /// <returns>true on success.</returns>
        Task<bool> DeleteAsync(Guid userId, Guid carId);

        /// <summary>
        /// Returns total number of cars.
        /// </summary>
        /// <returns>Total number of cars.</returns>
        Task<int> GetNumberOfCarsAsync();
    }
}
