using Dapper;
using DapperContext.Application.Repositories;
using DapperContext.Persistence.Context;
using System.Data;

namespace DapperContext.Persistence.Repositories
{
    public class GenericRepository(ContextDapper contextDapper) : IGenericRepository
    {
        private readonly ContextDapper _contextDapper = contextDapper ?? throw new ArgumentNullException(nameof(contextDapper));
        public IDbTransaction Transaction => _contextDapper.Transaction!;

        public async Task<IEnumerable<T>> GetAllAsyncWithStoredProcedure<T>(string SPName, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await _contextDapper.Connection!.QueryAsync<T>(SPName, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public async Task<bool> QueryAddAsyncByStoredProcedure(string SPName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
        {
            long affectedRows;
            try
            {
                affectedRows = await _contextDapper.Connection!.ExecuteAsync(SPName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRows > 0;
        }

        public async Task<bool> QueryDeleteAsyncByStoredProcedure(string SPName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
        {
            long affectedRows;
            try
            {
                affectedRows = await _contextDapper.Connection!.ExecuteAsync(SPName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRows > 0;
        }

        public async Task<IEnumerable<T>> QueryGetAsyncByStoredProcedure<T>(string SPName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await _contextDapper.Connection!.QueryAsync<T>(SPName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public async Task<T> QueryUpdateAsyncByStoredProcedure<T>(string SPName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            T user;
            try
            {
                user = await _contextDapper.Connection!.QuerySingleAsync<T>(SPName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }

            return user!;
        }
    }
}

