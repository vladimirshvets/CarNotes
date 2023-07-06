using Neo4j.Driver;

namespace CarNotes.Persistence.Neo4j
{
    public interface INeo4jDataAccess
    {
        /// <summary>
        /// Execute read transaction.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Collection that represents the result.</returns>
        Task<IEnumerable<IRecord>> ExecuteReadTransactionAsync(
            string query,
            IDictionary<string, object?>? parameters = null);

        /// <summary>
        /// Execute write transaction.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Collection that represents the result.</returns>
        Task<IEnumerable<IRecord>> ExecuteWriteTransactionAsync(
            string query,
            IDictionary<string, object?>? parameters = null);
    }
}
