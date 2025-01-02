using DapperContext.Context;
using DapperContext.UnitOfWork;

namespace DapperContext.Application.UnitOfWork
{
    public class UnitOfWork(ContextDapper contextDapper) : IUnitOfWork
    {
        private readonly ContextDapper _contextDapper = contextDapper ?? throw new ArgumentNullException(nameof(contextDapper));

        /// <summary>
        /// Method for begin the transaction
        /// </summary>
        /// <param name="function">Function for the transaction</param>
        /// <param name="cancellationToken">Cancellation token for stop transaction</param>
        /// <returns></returns>
        public async Task BeginTransaction(Func<CancellationToken, Task> function, CancellationToken cancellationToken)
        {
            _contextDapper.Connection!.Open();
            _contextDapper.Transaction = _contextDapper.Connection.BeginTransaction();
            try
            {
                await function(cancellationToken);
                Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                if (_contextDapper.Transaction != null && _contextDapper.Connection != null)
                {
                    _contextDapper.Transaction.Dispose();
                    _contextDapper.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Method for commit the transaction
        /// </summary>
        public void Commit()
        {
            try
            {
                _contextDapper.Transaction!.Commit();
            }
            catch
            {
                _contextDapper.Transaction!.Rollback();
                throw;
            }
            finally
            {
                _contextDapper.Transaction?.Dispose();
                _contextDapper.Connection!.Close();
            }
        }

        /// <summary>
        /// Method for the rollback of transaction
        /// </summary>
        public void Rollback()
        {
            _contextDapper.Transaction?.Rollback();
            _contextDapper.Transaction?.Dispose();
            _contextDapper.Connection!.Close();
        }
    }
}