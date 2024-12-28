using System.Data;

namespace DapperContext.Application.Repositories
{
    public interface IGenericRepository
    {
        IDbTransaction Transaction { get; }
        Task<IEnumerable<T>> GetAllAsyncWithStoredProcedure<T>(string spName, CommandType commandType) where T : class;
        Task<IEnumerable<T>> QueryGetAsyncByStoredProcedure<T>(string spName, object parameters, CommandType commandType) where T : class;
        Task<bool> QueryDeleteAsyncByStoredProcedure(string spName, object parameters, CommandType commandType);
        Task<bool> QueryAddAsyncByStoredProcedure(string spName, object parameters, CommandType commandType);
        Task<T> QueryUpdateAsyncByStoredProcedure<T>(string spName, object parameters, CommandType commandType) where T : class;
        Task<bool> ExecuteStoredProcedureAsyncByTvp<T>(string spName, string tableParameterName, string tvpName, IEnumerable<T> parameters, CommandType commandType) where T : class;
    }
}
