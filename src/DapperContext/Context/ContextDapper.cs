using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperContext.Context
{
    public sealed class ContextDapper(string connectionString)
    {
        private readonly string? _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        private IDbTransaction? _dbTransaction;
        private IDbConnection? _dbConnection;

        public IDbConnection? Connection
        {
            get
            {
                if (_dbConnection is null || _dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection = new SqlConnection(_connectionString);
                }
                return _dbConnection;
            }
        }

        public IDbTransaction? Transaction
        {
            get
            {
                return _dbTransaction;
            }
            set
            {
                _dbTransaction = value;
            }
        }
    }
}
