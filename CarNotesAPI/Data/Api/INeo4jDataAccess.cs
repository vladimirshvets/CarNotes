namespace CarNotesAPI.Data.Api
{
    public interface INeo4jDataAccess : IAsyncDisposable
    {
        /// <summary>
        /// Execute read list as an asynchronous operation.
        /// </summary>
        Task<List<string>> ExecuteReadListAsync(
            string query,
            List<string> returnObjectKey,
            IDictionary<string, object>? parameters = null);

        /// <summary>
        /// Execute read dictionary as an asynchronous operation.
        /// </summary>
        Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(
            string query,
            List<string> returnObjectKey,
            IDictionary<string, object>? parameters = null);

        /// <summary>
        /// Execute read scalar as an asynchronous operation.
        /// </summary>
        Task<T> ExecuteReadScalarAsync<T>(
            string query,
            IDictionary<string, object>? parameters = null);

        /// <summary>
        /// Execute write transaction with a dictionary result
        /// as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Primitive type (struct)</typeparam>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Dictionary representation of result.</returns>
        Task<T> ExecuteWriteWithScalarResultAsync<T>(
            string query,
            IDictionary<string, object>? parameters = null) where T : struct;

        /// <summary>
        /// Execute write transaction with a dictionary result
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Dictionary representation of result.</returns>
        Task<Dictionary<string, object>> ExecuteWriteWithDictionaryResultAsync(
            string query,
            IDictionary<string, object>? parameters = null);

        /// <summary>
        /// Execute write transaction with a list result
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>List representation of result.</returns>
        Task<List<Dictionary<string, object>>> ExecuteWriteWithListResultAsync(
            string query,
            IDictionary<string, object>? parameters = null);
    }
}
