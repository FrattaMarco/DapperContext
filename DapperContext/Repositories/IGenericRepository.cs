using System.Data;

namespace DapperContext.Application.Repositories
{
    public interface IGenericRepository
    {
        IDbTransaction Transaction { get; }
        Task<IEnumerable<T>> GetAllAsyncWithStoredProcedure<T>(string SPName, CommandType commandType) where T : class;
        Task<IEnumerable<T>> QueryGetAsyncByStoredProcedure<T>(string SPName, object parameters, CommandType commandType) where T : class;
        Task<bool> QueryDeleteAsyncByStoredProcedure(string SPName, object parameters, CommandType commandType);
        Task<bool> QueryAddAsyncByStoredProcedure(string SPName, object parameters, CommandType commandType);
        Task<T> QueryUpdateAsyncByStoredProcedure<T>(string SPName, object parameters, CommandType commandType) where T : class;
    }
}
