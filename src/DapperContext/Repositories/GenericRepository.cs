using Dapper;
using DapperContext.Context;
using System.Data;

namespace DapperContext.Repositories
{
    public class GenericRepository(ContextDapper contextDapper) : IGenericRepository
    {
        private readonly ContextDapper _contextDapper = contextDapper ?? throw new ArgumentNullException(nameof(contextDapper));
        public IDbTransaction Transaction => _contextDapper.Transaction!;

        #region Table valued Parameter
        /// <summary>
        /// Executes a Stored Procedure passing a tvp and its parameters 
        /// </summary>
        /// <typeparam name="T">Object of the table type</typeparam>
        /// <param name="spName">Stored Procedure's name</param>
        /// <param name="tvpName">Table type's name</param>
        /// <param name="tableParameterName">The name of the tvp parameter inside the stored procedure</param>
        /// <param name="parameters">List of tvp objects</param>
        /// <param name="commandTypeStoredProcedure">Command type</param>
        /// <returns>A Boolean variable indicating the SP was successfully executed</returns>
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
        #endregion

        #region Stored Procedure
        /// <summary>
        /// Get all records with a stored procedure
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="spName">Stored procedure's name</param>
        /// <param name="commandTypeStoredProcedure">Command type</param>
        /// <returns>A list of records of type <typeparamref name="T"/> obtained from the stored procedure </returns>
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

        /// <summary>
        /// It inserts records through a stored procedure to which parameters are passed
        /// </summary>
        /// <param name="spName">Stored Procedure's name</param>
        /// <param name="parameters">Parameters to pass to the stored procedure</param>
        /// <param name="commandTypeStoredProcedure">Command type</param>
        /// <returns>A Boolean variable indicating the SP was successfully executed</returns>
        public async Task<bool> AddAsyncWithStoredProcedure(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
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

        /// <summary>
        /// It deletes records through a stored procedure to which parameters are passed
        /// </summary>
        /// <param name="spName">Stored Procedure's name</param>
        /// <param name="parameters">Parameters to pass to the stored procedure</param>
        /// <param name="commandTypeStoredProcedure">Command type</param>
        /// <returns>A Boolean variable indicating the SP was successfully executed</returns>
        public async Task<bool> DeleteAsyncWithStoredProcedure(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure)
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

        /// <summary>
        /// It gets records through a stored procedure to which parameters are passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTypeStoredProcedure"></param>
        /// <returns>A list of records of type <typeparamref name="T"/> obtained from the stored procedure </returns>
        public async Task<IEnumerable<T>> GetAsyncWithStoredProcedure<T>(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
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

        /// <summary>
        /// It updates records through a stored procedure to which parameters are passed
        /// </summary>
        /// <param name="spName">Stored Procedure's name</param>
        /// <param name="parameters">Parameters to pass to the stored procedure</param>
        /// <param name="commandTypeStoredProcedure">Command type</param>
        /// <returns>The record updated of type <typeparamref name="T"/> obtained from the stored procedure </returns>
        public async Task<T> UpdateAsyncWithStoredProcedure<T>(string spName, object parameters, CommandType commandTypeStoredProcedure = CommandType.StoredProcedure) where T : class
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
        #endregion

        #region Query
        /// <summary>
        /// Gets records using a query
        /// </summary>
        /// <typeparam name="T">The object's type</typeparam>
        /// <param name="query">Query in string format</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>A collection from DB that satisfies the query</returns>
        public async Task<IEnumerable<T>> GetAsyncWithQuery<T>(string query, object parameters = null!) where T : class
        {
            IEnumerable<T> result;
            try
            {
                result = await _contextDapper.Connection!.QueryAsync<T>(query, parameters, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <param name="query">Query in string format</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>A Boolean value. True if the query was executed, false if it was not executed</returns>
        public async Task<bool> ExecuteAsyncWithQuery(string query, object parameters = null!)
        {
            int affectedRows;

            try
            {
                affectedRows = await _contextDapper.Connection!.ExecuteAsync(query, parameters, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRows != 0;

        }

        /// <summary>
        /// Update a record using a query
        /// </summary>
        /// <typeparam name="T">The object's type</typeparam>
        /// <param name="query">Query in string format</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>The object updated</returns>
        public async Task<T> UpdateAsyncWithQuery<T>(string query, object parameters = null!) where T : class
        {
            T updatedObject;
            try
            {
                updatedObject = await _contextDapper.Connection!.QuerySingleAsync<T>(query, parameters, transaction: _contextDapper.Transaction);
            }
            catch (Exception)
            {
                throw;
            }

            return updatedObject;
        }
        #endregion

        #region Static methods for utilities
        /// <summary>
        /// Build a generic TVP type
        /// </summary>
        /// <typeparam name="T">Object of the table type</typeparam>
        /// <param name="parameters">Parameters for build a TVP</param>
        /// <returns></returns>
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
        #endregion
    }
}

