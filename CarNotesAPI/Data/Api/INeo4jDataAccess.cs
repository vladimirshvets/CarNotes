namespace CarNotesAPI.Data.Api
{
    public interface INeo4jDataAccess : IAsyncDisposable
    {
        Task<List<string>> ExecuteReadListAsync(
            string query,
            string returnObjectKey,
            IDictionary<string, object>? parameters = null);

        Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(
            string query,
            string returnObjectKey,
            IDictionary<string, object>? parameters = null);

        Task<T> ExecuteReadScalarAsync<T>(
            string query,
            IDictionary<string, object>? parameters = null);

        Task<T> ExecuteWriteWithScalarResultAsync<T>(
            string query,
            IDictionary<string, object>? parameters = null) where T : struct;

        Task<Dictionary<string, object>> ExecuteWriteWithDictionaryResultAsync(
            string query,
            IDictionary<string, object>? parameters = null);

        Task<List<Dictionary<string, object>>> ExecuteWriteWithListResultAsync(
            string query,
            IDictionary<string, object>? parameters = null);
    }
}
