using System.Data;

namespace DapperContext.Repositories
{
    public interface IGenericRepository
    {
        IDbTransaction Transaction { get; }
        Task<IEnumerable<T>> GetAllAsyncWithStoredProcedure<T>(string spName, CommandType commandType) where T : class;
        Task<IEnumerable<T>> GetAsyncWithStoredProcedure<T>(string spName, object parameters, CommandType commandType) where T : class;
        Task<bool> DeleteAsyncWithStoredProcedure(string spName, object parameters, CommandType commandType);
        Task<bool> AddAsyncWithStoredProcedure(string spName, object parameters, CommandType commandType);
        Task<T> UpdateAsyncWithStoredProcedure<T>(string spName, object parameters, CommandType commandType) where T : class;
        Task<bool> ExecuteStoredProcedureAsyncByTvp<T>(string spName, string tableParameterName, string tvpName, IEnumerable<T> parameters, CommandType commandType) where T : class;
        Task<IEnumerable<T>> GetAsyncWithQuery<T>(string query, object parameters) where T : class;
        Task<bool> ExecuteAsyncWithQuery(string query, object parameters);
        Task<T> UpdateAsyncWithQuery<T>(string query, object parameters) where T : class;
    }
}
