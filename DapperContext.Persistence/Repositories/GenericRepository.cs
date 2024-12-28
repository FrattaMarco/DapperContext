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
        public async Task<bool> ExecuteStoredProcedureAsyncByTvp<T>(string spName, string tvpName, string tableParameterName, IEnumerable<T> parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            DataTable dt = BuildTvp(parameters);
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(tvpName, dt.AsTableValuedParameter(tableParameterName));

            try
            {
                await _contextDapper.Connection!.ExecuteAsync(spName, dynamicParameters, transaction: _contextDapper.Transaction, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
        public async Task<IEnumerable<T>> GetAllAsyncWithStoredProcedure<T>(string spName, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await _contextDapper.Connection!.QueryAsync<T>(spName, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<bool> QueryAddAsyncByStoredProcedure(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
        {
            long affectedRows;
            try
            {
                affectedRows = await _contextDapper.Connection!.ExecuteAsync(spName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRows > 0;
        }
        public async Task<bool> QueryDeleteAsyncByStoredProcedure(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
        {
            long affectedRows;
            try
            {
                affectedRows = await _contextDapper.Connection!.ExecuteAsync(spName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRows > 0;
        }
        public async Task<IEnumerable<T>> QueryGetAsyncByStoredProcedure<T>(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await _contextDapper.Connection!.QueryAsync<T>(spName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<T> QueryUpdateAsyncByStoredProcedure<T>(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
        {
            T updatedObject;
            try
            {
                updatedObject = await _contextDapper.Connection!.QuerySingleAsync<T>(spName, parameters, commandType: commandTypeStoredProcedure, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }

            return updatedObject!;
        }
        public static DataTable BuildTvp<T>(IEnumerable<T> parameters) where T : class
        {
            DataTable dt = new();
            var properties = typeof(T).GetProperties().ToList();

            properties.ForEach(x => dt.Columns.Add(x.Name, Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType));

            parameters.ToList().ForEach(x =>
            {
                DataRow row = dt.NewRow();
                properties.ForEach(p => row[p.Name] = p.GetValue(x) ?? DBNull.Value);
                dt.Rows.Add(row);
            });

            return dt;
        }
    }
}

